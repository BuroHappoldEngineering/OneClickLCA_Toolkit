import numpy as np

def is_valid_id(project_df):
    duplicate_ids = project_df[project_df['id'].duplicated(keep=False)]['id'].unique()
    if len(duplicate_ids) > 0:
        print(f"The 'id' column contains {len(duplicate_ids)} duplicated value(s): {', '.join(duplicate_ids[:10])}{'...' if len(duplicate_ids) > 10 else ''}. Please ensure that all 'id' values are unique before running the EPD mapping")
    else:
        return project_df