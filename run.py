import os
import subprocess

import os.path

def buildPath(*seqs: str) -> str :
    return os.getcwd() + os.path.sep + os.path.sep.join(seqs)

exePath = buildPath("heroku_output","MathQuizBot.exe")

subprocess.call(exePath)