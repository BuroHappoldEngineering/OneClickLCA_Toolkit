import os
import shutil
import tempfile
import chromadb

def create_chroma_client():
    source_chroma = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'Chroma_Storage')
    chroma_path = os.path.join(tempfile.gettempdir(), 'Chroma_Storage')
    if not os.path.exists(chroma_path):
        shutil.copytree(source_chroma, chroma_path)
    
    return chromadb.PersistentClient(path=chroma_path)