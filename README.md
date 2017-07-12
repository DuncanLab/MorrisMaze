# MorrisMaze
This is a simulation of the Morris Maze experiment written mostly in Unity C# with statistic calculations done in Python.


##### Configuration Files
The initial configuration is set by the [input.json](Assets/InputFiles~/input.json) file. Note that due to limitations of Unity's JSON library, it is recommended that you use a JSON formatter as provided in editors like sublime text before attempting to edit the file directly. Otherwise, you can update the values of the JSon file in the unity editor. Note that the file will be constantly read to and read from throughout the execution of an experiment. For documentation on each field, please see [this file](Assets/Scripts/DataSingleton.cs) which contains the specifics on each field within the JSon.


For the configuration on experiments, it will be set by a `csv` file setup in the InputFiles~ directory. This file will contain 5 numbers separated by commas similar to as follows:

```
0, 10, 0, 4, 3
```

where:

- 0 means that it is environment number 0
- 10 means that the trial will last 10 seconds
- 0 means that it will use the first pickup given by input.json
- 4 means that the number of walls in the initial configuration is 4
- 3 means that the minimum time (outside of loading) between trials will be 3 seconds


##### Python files
Python files go starting in relative path to Assets/InputFiles~. Python files need to be able to be executed as follows:

``` python
>>> python Example.py <pickup_type> <trial_number>
>>> -5, -5

```

where `pickup_type` is given as the tag of the pick up from input.json and the `trial_number` is given as an integer in the experiment starting at zero. The output of the file is expected to be two numbers separated by a comma.


##### List of Available Commands (Developer Mode Only):
1. [1] to INCREASE number of walls by offset
2. [2] to DECREASE number of walls by offset
3. [3] to get a random wall
4. [space] to commit your changes
5. [WASD and arrow keys] will work to control character
6. [G] to take a screen shot of the current game system
7. [H] to run through screen shots of the entire system
8. [F] to save the current file
9. [K and L] will switch between keys
10. [0] will reset to the initial start screen
11. [n] will begin an experiment