import requests
import pandas as pd
from pathlib import Path

# from epd_mapping.util.validation import is_valid_massconversion

def load_epd_library(epd_library_path):
    """Load EPD library and enrich with OneClick API data. Cached so API calls only run once.
    Returns (epd_library_df, missing_epds) where missing_epds lists OCLIDs not found via API."""

    epd_library_df = pd.read_json(epd_library_path)
    
    return epd_library_df #, missing_epds