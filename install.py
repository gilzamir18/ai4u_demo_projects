import urllib.request
import sys
import os

import zipfile

def delete_file(file_path):
    try:
        os.remove(file_path)
        print(f"File '{file_path}' deleted successfully!")
    except OSError as e:
        print(f"Error deleting file '{file_path}': {e}")

def unzip_zip(file_path, extract_to):
    with zipfile.ZipFile(file_path, 'r') as zip_ref:
        zip_ref.extractall(extract_to)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Use: python install.py ProjectPath")
        sys.exit(1)

    url = "https://raw.githubusercontent.com/gilzamir18/ai4u/main/packages/ai4u.zip"
    file_path =  os.path.join(f"{sys.argv[1]}", "addons", "ai4u.zip")
    unziped_path = os.path.join(f"{sys.argv[1]}", "addons")

    with urllib.request.urlopen(url) as file, open(file_path, 'wb') as f:
        f.write(file.read())

    unzip_zip(file_path, unziped_path)
    delete_file(file_path)
    print(f"AI4U installed in {file_path}!")