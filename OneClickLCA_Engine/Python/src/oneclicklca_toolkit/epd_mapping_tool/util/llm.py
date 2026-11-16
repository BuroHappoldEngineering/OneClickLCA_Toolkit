import pandas as pd

def llm_questions(project_df):
    questions_project = []

    for index, row in project_df.iterrows():
        question = {
            "id": row['id'],
            "Costplan entry": row['Costplan entry'],
            "Headline 1": row['Headline 1'],
            "Headline 2": row['Headline 2'],
            "Headline 3": row['Headline 3'],
            "Headline 4": row['Headline 4'],
            "Headline 5": row['Headline 5'],
        }
        questions_project.append(question)
    return questions_project