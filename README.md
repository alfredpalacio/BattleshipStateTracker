# BattleshipStateTracker

## General Info
Created a simple Battleship State Tracker API

## How to use the program
- Just build the program and run
- You can these endpoints:

| Endpoint | Method | Example | 
| ------ | ------ |
| Create | GET | https://localhost:5001/api/Tracker/create |
| Add | POST | https://localhost:5001/api/Tracker/add |
| AddRandom | POST | https://localhost:5001/api/Tracker/addRandom |
| Attack | GET | https://localhost:5001/api/Tracker/attack?xCoordinate=4&yCoordinate=7 |
| Status | GET | https://localhost:5001/api/Tracker/status |

### Example payload for Add endpoint
```
{
    "XCoordinate":1,
    "YCoordinate":1,
    "Length":9,
    "IsHorizontal":true
}
```

## Unit tests
I've added a few which can be ran after building the solution. Thank you.
