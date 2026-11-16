import pandas as pd
from pathlib import Path

from epd_library import load_epd_library

def filter_examples(project_examples_path):
    epd_library_df, _ = load_epd_library()
    examples_df = pd.read_json(project_examples_path)
    examples_epdlibrary = examples_df[examples_df['OCLID'].isin(epd_library_df['OCLID'])].reset_index(drop=True)
    examples_remaining = examples_df[~examples_df['OCLID'].isin(epd_library_df['OCLID'])].reset_index(drop=True)

    return examples_epdlibrary, examples_remaining

def examples(project_examples_path):
    examples_df = pd.read_json(project_examples_path)
    return examples_df