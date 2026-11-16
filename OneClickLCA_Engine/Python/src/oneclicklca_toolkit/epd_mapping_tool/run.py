import json
import logging
import sys
import pandas as pd
from pathlib import Path

from python_toolkit.bhom.logging import CONSOLE_LOGGER

from compute.epd_library import load_epd_library
from compute.examples import filter_examples, examples
from util.nested_dict import tree, nested_append, to_dict
from util.validation import is_valid_id
from util.llm import llm_questions
from clients.chroma_db import create_chroma_client

# TODO Update below to Python_Toolit logger if needed - For analytics?
# def _get_console_logger() -> logging.Logger:
#     logger = logging.getLogger("oneclicklca_toolkit.epd_mapping_tool")
#     if not logger.handlers:
#         handler = logging.StreamHandler(sys.stdout)
#         handler.setFormatter(logging.Formatter("%(asctime)s - %(levelname)s - %(message)s"))
#         logger.addHandler(handler)
#         logger.setLevel(logging.INFO)
#     return logger


# CONSOLE_LOGGER = _get_console_logger()

# ----------------------------------------------------------------------------
# Config
# ----------------------------------------------------------------------------
DATASETS_FOLDER = Path(__file__).parent / "datasets"
DICTIONARY_PATH = DATASETS_FOLDER / "dictionary.xlsx"  # LLM input
EPD_LIBRARY_PATH = DATASETS_FOLDER / "epd_library_api_TEMP.json"
PROJECT_EXAMPLES_PATH = DATASETS_FOLDER / "projects_filtered.json"
#TODO Set up input and output folder to be folder links from the user
PROJECT_INPUT_PATH = Path(r"C:\Users\jolin\OneDrive - Buro Happold\02_CompDev\EPD Mapping\03_BHoM_Move\input\project_input_10.xlsx")
# OUTPUT_FOLDER = Path(__file__).parent / "output"
# OUTPUT_FILE = OUTPUT_FOLDER / "epd_mapping_results.xlsx"
# EPDS_NOT_IN_API_FILE = OUTPUT_FOLDER / "epds_not_in_api.xlsx"

N_RESULTS = 20  # number of vector search matches per collection
MAX_DISTANCE = 0.5  # matches with a distance at or above this are removed


# ----------------------------------------------------------------------------
# Load inputs
# ----------------------------------------------------------------------------
#TODO Add EPD library functions when the outputs are available in the One Click Materials API
def load_epd_library_data(epd_library_path: Path):
#     """Load EPD Library with API additions. Returns (epd_lib_df, epds_not_in_api)."""
#     epd_lib_tuple = load_epd_library()
#     epd_lib_df = epd_lib_tuple[0]
#     epds_not_in_api = epd_lib_tuple[1]
    epd_lib_df = load_epd_library(epd_library_path)
    return epd_lib_df #, epds_not_in_api


def build_epd_hierarchy(epd_lib_df) -> str:
    """Build nested EPD category dict for the LLM prompt - LLM Input."""
    hier = tree()

    for _, row in epd_lib_df.iterrows():
        keys = [row.resourceType, row.resourceSubType, row.resourceTypeL3, row.technicalSpec]
        nested_append(hier, keys, list([row.staticFullName, row.carbonFactorPerKG]))

    epd_lib_dict = json.dumps(to_dict(hier), indent=2)  # LLM input
    return epd_lib_dict


def load_dictionary(dictionary_path: Path):
    """Load supporting dictionary - LLM Input."""
    df_dict = pd.read_excel(dictionary_path)  # LLM input
    return df_dict


def build_example_collections(project_examples_path: Path):
    """Filter examples to EPD Library and non EPD Library, and store example embeddings in chroma databases. Returns (ex_epd_lib, ex_remaining, collection_epd_lib, collection_remaining)."""
    ex_epd_lib, ex_remaining = filter_examples(project_examples_path)

    client_chroma = create_chroma_client()

    collection_epd_lib = create_collection(chroma_client=client_chroma, collection_name="examples_epd_library", collection_df=ex_epd_lib)
    collection_remaining = create_collection(chroma_client=client_chroma, collection_name="examples_remaining", collection_df=ex_remaining)
    return ex_epd_lib, ex_remaining, collection_epd_lib, collection_remaining

# TODO Move to main?
def load_project_input(project_input_path: Path):
    """Get input excel sheet, make id into string and check if valid - USER INPUT."""
    project_df = pd.read_excel(project_input_path)
    project_df['id'] = project_df['id'].astype(str)
    return project_df

# TODO Requires openAI models
def create_questions(project_df):
    """ Set up llm and create questions - LLM INPUT. Returns (llm, questions, question_embeddings)."""
    llm = get_openai_chat_client()
    questions = llm_questions(project_df)

    # Make embeddings out of the questions for the example vector search
    # question_texts = [make_embedding_text(q) for q in questions]
    # question_embeddings = generate_embeddings_batch(question_texts)
    return llm, questions, #question_embeddings


# ----------------------------------------------------------------------------
# EPD-mapping loop
# ----------------------------------------------------------------------------
# def query_matched_ids(collection, query_embedding) -> list:
#     """Vector search to the best matches and remove results with matches worse than MAX_DISTANCE."""
#     results = collection.query(
#         query_embeddings=[query_embedding],
#         n_results=N_RESULTS,
#         include=["metadatas", "distances"]
#     )
#     matched_ids = [
#         id for id, dist in zip(results["ids"][0], results["distances"][0])
#         if dist < MAX_DISTANCE
#     ]
#     return matched_ids


# def run_mapping_loop(
#     llm,
#     questions,
#     question_embeddings,
#     collection_epd_lib,
#     collection_remaining,
#     ex_epd_lib,
#     ex_remaining,
#     epd_lib_dict,
#     df_dict,
#     row,
# ) -> list:
#     """Run epd-mapping loop for each question. Returns the raw LLM responses (one string per question).

#     `row` is passed to llm_example_dict exactly as the notebook did: the notebook never
#     re-assigned it in the loop, so it is the last row left over from the iterrows loop in
#     build_epd_hierarchy().
#     """
#     responses = []

#     for i, question in enumerate(questions):
#         CONSOLE_LOGGER.info(f"Mapping question {i + 1} of {len(questions)}...")
#         query_embedding = question_embeddings[i]

#         # Set up EPD library examples - vector search to best matches
#         matched_ids_epdlibrary = query_matched_ids(collection_epd_lib, query_embedding)
#         epdlibrary_examples = llm_example_dict(row, ex_epd_lib, matched_ids_epdlibrary)

#         # Set up remaining examples
#         matched_ids_remaining = query_matched_ids(collection_remaining, query_embedding)
#         remaining_examples = llm_example_dict(row, ex_remaining, matched_ids_remaining)

#         # Run LLM prompt
#         user_message = build_user_message(question, epd_lib_dict, epdlibrary_examples, remaining_examples)
#         system_message = build_system_message(df_dict)

#         response = llm.invoke([
#             {"role": "system", "content": system_message},
#             {"role": "user", "content": user_message}
#         ])
#         responses.append(response.content)

#     return responses


# ----------------------------------------------------------------------------
# Post-processing
# ----------------------------------------------------------------------------
# def build_response_df(responses):
#     """Add responses in dataframe."""
#     parsed_responses = [json.loads(r) for r in responses]
#     response_df = pd.DataFrame(parsed_responses)
#     return response_df


# def add_database_and_project_info(response_df, project_df, epd_lib_df):
#     """Add back the unit and quantity from the project dataframe, then the carbon factors and OCLIDs."""
#     response_df = add_project_info(response_df, project_df)

#     examples_df = examples()
#     response_df = add_database_info(response_df, epd_lib_df, examples_df)
#     return response_df


# def save_results(response_df, epds_not_in_api):
#     """Save the final mapping table, and the EPDs not in the API (notebook output 1)."""
#     OUTPUT_FOLDER.mkdir(parents=True, exist_ok=True)

#     response_df.to_excel(OUTPUT_FILE, index=False)
#     CONSOLE_LOGGER.info(f"Saved EPD mapping results to: {OUTPUT_FILE}")

#     if isinstance(epds_not_in_api, (pd.DataFrame, pd.Series)):
#         epds_not_in_api.to_excel(EPDS_NOT_IN_API_FILE)
#     else:
#         pd.DataFrame({"epds_not_in_api": list(epds_not_in_api)}).to_excel(EPDS_NOT_IN_API_FILE, index=False)
#     CONSOLE_LOGGER.info(f"Saved EPDs not in API to: {EPDS_NOT_IN_API_FILE}")


# # ----------------------------------------------------------------------------
# # Entry point
# # ----------------------------------------------------------------------------
def main():
#     CONSOLE_LOGGER.info("Loading EPD library...")
#     try:
#         epd_lib_df, epds_not_in_api = load_epd_library_data()
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to load the EPD library.", exc_info=1)
#         return
#     CONSOLE_LOGGER.info(f"EPDs not in API: {epds_not_in_api}")  # OUTPUT 1

#     CONSOLE_LOGGER.info("Building EPD category hierarchy...")
#     epd_lib_dict = build_epd_hierarchy(epd_lib_df)
#     # The notebook's `row` variable in the mapping loop is the last row of the iterrows loop above
#     last_epd_row = epd_lib_df.iloc[-1]

    # CONSOLE_LOGGER.info("Loading supporting dictionary...")
    df_dict = load_dictionary(DICTIONARY_PATH)

#     CONSOLE_LOGGER.info("Setting up example collections...")
#     try:
#         ex_epd_lib, ex_remaining, collection_epd_lib, collection_remaining = build_example_collections()
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to set up the example collections.", exc_info=1)
#         return

#     CONSOLE_LOGGER.info("Loading project input...")
    try:
        project_df = load_project_input(PROJECT_INPUT_PATH)
        project_df = is_valid_id(project_df)
        CONSOLE_LOGGER.info(project_df)
    except Exception:
        CONSOLE_LOGGER.error("Error when trying to load the project input.", exc_info=1)
        return

#     CONSOLE_LOGGER.info("Setting up LLM and creating questions...")
#     try:
#         llm, questions, question_embeddings = create_questions(project_df)
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to create the LLM questions.", exc_info=1)
#         return

#     CONSOLE_LOGGER.info("Running EPD mapping...")
#     try:
#         responses = run_mapping_loop(
#             llm=llm,
#             questions=questions,
#             question_embeddings=question_embeddings,
#             collection_epd_lib=collection_epd_lib,
#             collection_remaining=collection_remaining,
#             ex_epd_lib=ex_epd_lib,
#             ex_remaining=ex_remaining,
#             epd_lib_dict=epd_lib_dict,
#             df_dict=df_dict,
#             row=last_epd_row,
#         )
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to run the EPD mapping loop.", exc_info=1)
#         return

#     CONSOLE_LOGGER.info("Processing responses...")
#     try:
#         response_df = build_response_df(responses)
#         response_df = add_database_and_project_info(response_df, project_df, epd_lib_df)
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to process the LLM responses.", exc_info=1)
#         return

#     save_results(response_df, epds_not_in_api)


if __name__ == "__main__":
    main()