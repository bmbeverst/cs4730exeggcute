# Made by Moops on 5/04/011
# code is released into the public domain
# usage is `python rename.py path-to-Application-Files [-q]`
# Renames files output by XNA so you don't have to install the application
# to run it
import os, re, sys

deploy = re.compile("deploy")
SAFETY = 150

def grab_files(directory):
    names = []
    for dirname, dirnames, filenames in os.walk(directory):
        for file in filenames:
            filename = os.path.join(dirname, file)
            if not os.path.isdir(filename) and deploy.search(file):
                names.append(filename)
    return names
def recurse_rename(directory, quiet):
    filenames = grab_files(directory)
    if len(filenames) > SAFETY:
        print("Number of files is greater than 150, are you sure you picked" +
              " the right directory? (y/n)")
        response = input()
        if not (response[0] == 'y' or response[0] == 'Y'):
            sys.exit("aborted")
    if len(filenames) == 0:
        sys.exit("No files found, aborting.")
    for file in filenames:
        newname = file[:-7]
        if not quiet:
            print("Renaming " + file + " to "+ newname)
        os.rename(file, newname)


def main():
    quiet = False
    if len(sys.argv) < 2:
        sys.exit("Usage is `python rename.py rel-path-to-Application-Files`")
    if len(sys.argv) >= 3:
        quiet = (sys.argv[2][1] == "q")
    dirname = sys.argv[1]
    recurse_rename(sys.argv[1], quiet)        
    
main()
