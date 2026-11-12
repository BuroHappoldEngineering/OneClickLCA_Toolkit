import json
import logging
import sys
import pandas as pd
from pathlib import Path

# import oneclicklca_toolkit



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
#TODO Set up input and output folder to be folder links from the user
# PROJECT_INPUT_PATH = Path(r"/Workspace/Users/johanna.olin@burohappold.com/epd_mapping_bhom_mockup/project_input_10.xlsx")
# OUTPUT_FOLDER = Path(__file__).parent / "output"
# OUTPUT_FILE = OUTPUT_FOLDER / "epd_mapping_results.xlsx"
# EPDS_NOT_IN_API_FILE = OUTPUT_FOLDER / "epds_not_in_api.xlsx"

N_RESULTS = 20  # number of vector search matches per collection
MAX_DISTANCE = 0.5  # matches with a distance at or above this are removed


# ----------------------------------------------------------------------------
# Load inputs
# ----------------------------------------------------------------------------
#TODO Add EPD library functions when the outputs are available in the One Click Materials API
# def load_epd_library_data():
#     """Load EPD Library with API additions. Returns (epd_lib_df, epds_not_in_api)."""
#     epd_lib_tuple = load_epd_library()
#     epd_lib_df = epd_lib_tuple[0]
#     epds_not_in_api = epd_lib_tuple[1]
#     return epd_lib_df, epds_not_in_api


# def build_epd_hierarchy(epd_lib_df) -> str:
#     """Build nested EPD category dict for the LLM prompt - LLM Input."""
#     hier = tree()

#     for _, row in epd_lib_df.iterrows():
#         keys = [row.resourceType, row.resourceSubType, row.resourceTypeL3, row.technicalSpec]
#         nested_append(hier, keys, list([row.staticFullName, row.carbonFactorPerKG]))

#     epd_lib_dict = json.dumps(to_dict(hier), indent=2)  # LLM input
#     return epd_lib_dict


def load_dictionary(dictionary_path: Path):
    """Load supporting dictionary - LLM Input."""
    df_dict = pd.read_excel(dictionary_path)  # LLM input
    return df_dict

# TODO Requires EPD Library
# def build_example_collections():
#     """Filter examples to EPD Library and non EPD Library, and store example embeddings in chroma databases.

#     Returns (ex_epd_lib, ex_remaining, collection_epd_lib, collection_remaining).
#     """
#     ex_epd_lib, ex_remaining = filter_examples()

#     client_chroma = create_chroma_client()

#     collection_epd_lib = create_collection(chroma_client=client_chroma, collection_name="examples_epd_library", collection_df=ex_epd_lib)
#     collection_remaining = create_collection(chroma_client=client_chroma, collection_name="examples_remaining", collection_df=ex_remaining)
#     return ex_epd_lib, ex_remaining, collection_epd_lib, collection_remaining

#TODO THIS SHOULD BE POSSIBLE TO RUN
# def load_project_input(project_input_path: Path):
#     """Get input excel sheet, make id into string and check if valid - USER INPUT."""
#     project_df = pd.read_excel(project_input_path)
#     project_df['id'] = project_df['id'].astype(str)
#     is_valid_id(project_df)
#     return project_df

# TODO Requires openAI models
# def create_questions(project_df):
#     """Set up llm and create questions - LLM INPUT.

#     Returns (llm, questions, question_embeddings).
#     """
#     llm = get_openai_chat_client()
#     questions = llm_questions(project_df)

#     # Make embeddings out of the questions for the example vector search
#     question_texts = [make_embedding_text(q) for q in questions]
#     question_embeddings = generate_embeddings_batch(question_texts)
#     return llm, questions, question_embeddings


# ----------------------------------------------------------------------------
# Prompts
# ----------------------------------------------------------------------------
# def build_user_message(question, epd_lib_dict, epdlibrary_examples, remaining_examples) -> str:
#     user_message = f"""
#     Please select categories for the material described in the following dictionary: {question}. 

#     Use the Costplan entry, Headline 1, Headline 2, Headline 3, Headline 4, and Headline 5 to select one of the fixed categories under each of the following dictionary keys: resourceType, resourceSubType, resourceTypeL3, technicalSpec and EPD name.
    
#     The valid category structure is provided: {epd_lib_dict}. Note that the JSON no longer contains the original label names, but its hierarchy follows:
#     Level 0 = resourceType
#     Level 1 = resourceSubType
#     Level 2 = resourceTypeL3
#     Level 3 = technicalSpec
#     Level 4 = EPD name

#     Use this category structure to identify the correct category at each depth.
    
#     All categories will always have information and the valid category should always be mapped even if the category is null.

#     It is explicitly valid and expected for the same string to appear at multiple hierarchy levels (e.g. resourceType="insulation" AND resourceSubType="insulation"). When the hierarchy defines the same value at Level N and Level N+1, you MUST output that identical string for the fields with the same value. Do NOT substitute a different value or remove one of the values simply because it would "look" like a duplicate — the hierarchy is the source of truth and all the values should be filled in.

#     Be aware that the headlines are not always present, and may be empty.

#     Use the following examples to support the selection: {epdlibrary_examples['examples']}. Each example is an array following the schema order as stated here: {epdlibrary_examples['schema']}.

#     Where valid categories can't be reasonably selected from the valid category structure, select resourceType, resourceSubType, resourceTypeL3, technicalSpec and EPD name using the following examples instead: {remaining_examples['examples']}. Each example is an array following the schema order as stated here: {remaining_examples['schema']}.
#     Be aware that these examples will not always follow the valid category structure and if that is the case the category strucutre as provided in the example should be followed. All categories in the examples will always have information and the valid category should always be mapped even if the category is null or left empty.
#     """
#     return user_message


# def build_system_message(df_dict) -> str:
#     system_message = f"""
#     Role
#     You are a UK‑based building‑engineering carbon‑calculation assistant.
#     You are aware of the terms in the follwing dictionary: {df_dict}
    
#     Primary Task
#     Given a single construction costplan entry and headlines, select the required categories and return JSON only — no explanations or extra text.

#     Your task is NOT to classify or infer new labels.

#     Your task is to SELECT an EXISTING VALID PATH from the provided category structure.

#     Each of the following must be selected exactly from the JSON hierarchy provided:

#     resourceType must be selected from Level 0
#     resourceSubType must be a child of the selected resourceType at Level 1
#     resourceTypeL3 must be a child of the selected resourceSubType at Level 2
#     technicalSpec must be a child of the selected resourceTypeL3 at Level 3 OR be an empty string if missing
#     EPD name must be selected from Level 4 under the chosen technicalSpec

#     You are performing hierarchical path retrieval, not semantic classification.

#     If no exact valid path exists in the provided hierarchy:
#     select the closest EXISTING FULL PATH from the hierarchy.
#     DO NOT create, modify, infer, or generalise any label

#     Output Format
#     Return exactly one JSON object with the following keys, in this order, and no additional keys:
#     "id" — string
#     "Costplan entry" — string
#     "Headline 1" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)
#     "Headline 2" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)
#     "Headline 3" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)
#     "Headline 4" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)
#     "Headline 5" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)
#     "resourceType" — string (e.g., metal, timber, concrete, masonry, insulation, membrane, plasticMembraneRoofing)
#     "resourceSubType" — string (e.g., structuralSteel, softwood, rebar, aluminiumExtrusions)
#     "resourceTypeL3" — string (e.g., structuralSteelPlateSheetCoilsDecking)
#     "technicalSpec" — string (e.g., structuralSteelPlateSheetCoilsDecking)
#     "EPD name" — string (will vary in detail level depending on the material, e.g. 'Single skin wall from bricks, including mortar, 102.5 mm, 1:5 cement:sand mix, uses CEM I cement, 194.11 kg/m2' and 'Ceramic tile',)
    
#     Strict Rules
#     JSON only: 
#     Output must be a valid JSON object, not a string or code block. Do not include any extra spaces, indentation, or line breaks in the output. Do not wrap the JSON in quotes or markdown/code fences. Examples may be formatted for readability; output must be a single-line JSON object.
#     Schema completeness: 
#     Always select a category for all of the levels even if the value is the same for different levels.
#     Normalisation: Don't change the formatting of any of the outputs.
#     Hierarchy Enforcement:
#     All selected values must form a valid parent-child path in the provided category structure.
#     A value at Level N is ONLY valid if it exists as a child of the selected Level N-1 value in the hierarchy.
#     Closed Set Constraint:
#     All output values must be copied EXACTLY from the category structure.
#     String similarity, paraphrasing, or semantic equivalents are NOT permitted.
#     If the exact label is not present in the structure it MUST NOT be used.
#     """
#     return system_message


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
    print(df_dict)

#     CONSOLE_LOGGER.info("Setting up example collections...")
#     try:
#         ex_epd_lib, ex_remaining, collection_epd_lib, collection_remaining = build_example_collections()
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to set up the example collections.", exc_info=1)
#         return

#     CONSOLE_LOGGER.info("Loading project input...")
#     try:
#         project_df = load_project_input(PROJECT_INPUT_PATH)
#     except Exception:
#         CONSOLE_LOGGER.error("Error when trying to load the project input.", exc_info=1)
#         return

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