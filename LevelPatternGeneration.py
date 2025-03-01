# %% [markdown]
# Read inputs from file.

# %%
import json

with open('input_information.json') as f:
    data = json.load(f)

MAX_RANGE = data['grid_size'][0]
NUMBER_OF_ROOMS = data['number_of_rooms'][0]
PATH_WIDTH = data['path_width'][0]
TOWER_BOX_SIZE = data['tower_box_width'][0]
SPAWN_BOX_SIZE = data['spawn_box_width'][0]
INTERMEDIARY_BOX_SIZE = data['intermediary_box_width'][0]
GENERATE_SPAWN_BOX = data['generate_spawn_box'][0]

print(MAX_RANGE)
print(NUMBER_OF_ROOMS)
print(PATH_WIDTH)
print(TOWER_BOX_SIZE)
print(SPAWN_BOX_SIZE)
print(INTERMEDIARY_BOX_SIZE)
print(GENERATE_SPAWN_BOX)

# Rerun code until a valid image is printed.
rerunEverythingCheck = True
while (rerunEverythingCheck):
    rerunEverythingCheck = False


    # %% [markdown]
    # Function to assist with ensuring that coordinates don't generate on one side of a diagonal.
    # https://www.geeksforgeeks.org/check-whether-a-given-point-lies-inside-a-triangle-or-not/

    # %%
    # A utility function to calculate area 
    # of triangle formed by (x1, y1), 
    # (x2, y2) and (x3, y3)

    def area(x1, y1, x2, y2, x3, y3):

        return abs((x1 * (y2 - y3) + x2 * (y3 - y1) 
                    + x3 * (y1 - y2)) / 2.0)


    # A function to check whether point P(x, y)
    # lies inside the triangle formed by 
    # A(x1, y1), B(x2, y2) and C(x3, y3) 
    def isInside(x1, y1, x2, y2, x3, y3, x, y):

        # Calculate area of triangle ABC
        A = area (x1, y1, x2, y2, x3, y3)

        # Calculate area of triangle PBC 
        A1 = area (x, y, x2, y2, x3, y3)
        
        # Calculate area of triangle PAC 
        A2 = area (x1, y1, x, y, x3, y3)
        
        # Calculate area of triangle PAB 
        A3 = area (x1, y1, x2, y2, x, y)
        
        # Check if sum of A1, A2 and A3 
        # is same as A
        if(A == A1 + A2 + A3):
            return True
        else:
            return False

    # Driver program to test above function
    # Let us check whether the point P(10, 15)
    # lies inside the triangle formed by 
    # A(0, 0), B(20, 0) and C(10, 30)
    if (isInside(0, 0, 20, 0, 10, 30, 10, 15)):
        print('Inside')
    else:
        print('Not Inside') 

    # This code is contributed by Danish Raza


    # %% [markdown]
    # Randomly generate a set of (x, y) coordinates. These will serve as the center of the rooms.

    # %%
    import random
    import math

    RANDOM_MIRROR_CHOICE = random.randint(0, 7)
    MIN_RANGE = 0

    # Determine what part of the array points are generated in before being mirrored.
    def random_side(chosenComparison, x, y, maxValue):
        if (chosenComparison == 0):
            if (y < maxValue / 2):
                return True
            return False
        elif (chosenComparison == 1):
            if (y >= maxValue / 2):
                return True
            return False
        elif (chosenComparison == 2):
            if (x < maxValue / 2):
                return True
            return False
        elif (chosenComparison == 3):
            if (x >= maxValue / 2):
                return True
            return False
        elif (chosenComparison == 4):
            return isInside(0, 0, 100, 0, 0, 100, x, y)
        elif (chosenComparison == 5):
            return isInside(0, 0, 100, 0, 100, 100, x, y)
        elif (chosenComparison == 6):
            return isInside(100, 100, 100, 0, 0, 100, x, y)
        elif (chosenComparison == 7):
            return isInside(0, 0, 0, 100, 100, 100, x, y)
        else:
            return False


    # Generate the coordinates.
    def generate_coordinates(minCoordinates, maxCoordinates, numberOfCoordinates):
    
        x_min = minCoordinates[0]
        y_min = minCoordinates[1]

        x_max = maxCoordinates[0]
        y_max = maxCoordinates[1]

        # Generates random x and y coordinates within the given range.
        setOfCoordinates = []
        setOfXCoordinates = []
        setOfYCoordinates = []

        print(RANDOM_MIRROR_CHOICE)

        while (len(setOfCoordinates) < numberOfCoordinates):
            randomXCoordiante = random.randint(x_min, x_max)
            randomYCoordinate = random.randint(y_min, y_max)

            print(randomXCoordiante)
            print(randomYCoordinate)
            print()
            
            if (random_side(RANDOM_MIRROR_CHOICE, randomXCoordiante, randomYCoordinate, MAX_RANGE)):
                if (len(setOfCoordinates) == numberOfCoordinates - 1):
                    randomXCoordiante = math.ceil(x_max / 2)
                    randomYCoordinate = math.ceil(y_max / 2)

                setOfXCoordinates.append(randomXCoordiante)
                setOfYCoordinates.append(randomYCoordinate)
                setOfCoordinates.append((randomXCoordiante, randomYCoordinate))

        return (setOfXCoordinates, setOfYCoordinates, setOfCoordinates)

    # %% [markdown]
    # Convert coordinates to 0 and 1 grid used in A* Algorithm from GeeksForGeeks.

    # %%
    # Create 2D grid of zeros.
    rows, cols = MAX_RANGE + 1, MAX_RANGE + 1
    array_2d = [[1 for _ in range(cols)] for _ in range(rows)]
    coordinate_only_array_2d = [[0 for _ in range(cols)] for _ in range(rows)]

    # Add coordinates as 1 into the grid.
    returnedOutput = generate_coordinates((MIN_RANGE, MIN_RANGE), (MAX_RANGE, MAX_RANGE), NUMBER_OF_ROOMS)
    returnedSetOfCoordinates = returnedOutput[2]
    print(returnedSetOfCoordinates)

    for element in returnedSetOfCoordinates:
        currentXCoordinate = element[0]
        currentYCoordinate = element[1]
        array_2d[currentXCoordinate][currentYCoordinate] = 1
        coordinate_only_array_2d[currentXCoordinate][currentYCoordinate] = 1


    # Test print the grid.
    import numpy as np

    # Print the entire array without truncation
    np.set_printoptions(threshold=np.inf, linewidth=500)

    print(np.matrix(array_2d))

    # Restore default print options
    np.set_printoptions(threshold=1000, linewidth=75)

    # %% [markdown]
    # A* Algorithm from GeeksForGeeks.
    # https://www.geeksforgeeks.org/a-search-algorithm/

    # %%
    import math
    import heapq

    # Define the Cell class
    class Cell:
        def __init__(self):
            self.parent_i = 0  # Parent cell's row index
            self.parent_j = 0  # Parent cell's column index
            self.f = float('inf')  # Total cost of the cell (g + h)
            self.g = float('inf')  # Cost from start to this cell
            self.h = 0  # Heuristic cost from this cell to destination

    # Define the size of the grid
    # ROW = 9
    # COL = 10
    ROW = MAX_RANGE + 1
    COL = MAX_RANGE + 1

    # Check if a cell is valid (within the grid)
    def is_valid(row, col):
        return (row >= 0) and (row < ROW) and (col >= 0) and (col < COL)

    # Check if a cell is unblocked
    def is_unblocked(grid, row, col):
        return grid[row][col] == 1

    # Check if a cell is the destination
    def is_destination(row, col, dest):
        return row == dest[0] and col == dest[1]

    # Calculate the heuristic value of a cell (Euclidean distance to destination)
    def calculate_h_value(row, col, dest):
        return ((row - dest[0]) ** 2 + (col - dest[1]) ** 2) ** 0.5

    # Trace the path from source to destination
    def trace_path(cell_details, dest):
        print("The Path is ")
        path = []
        row = dest[0]
        col = dest[1]

        # Trace the path from destination to source using parent cells
        while not (cell_details[row][col].parent_i == row and cell_details[row][col].parent_j == col):
            path.append((row, col))
            temp_row = cell_details[row][col].parent_i
            temp_col = cell_details[row][col].parent_j
            row = temp_row
            col = temp_col

        # Add the source cell to the path
        path.append((row, col))

        returnedAStarPath = path

        # Reverse the path to get the path from source to destination
        path.reverse()

        # Print the path
        for i in path:
            print("->", i, end=" ")
        print()

        return returnedAStarPath

    # Implement the A* search algorithm
    def a_star_search(grid, src, dest):
        # Check if the source and destination are valid
        if not is_valid(src[0], src[1]) or not is_valid(dest[0], dest[1]):
            print("Source or destination is invalid")
            return

        # Check if the source and destination are unblocked
        if not is_unblocked(grid, src[0], src[1]) or not is_unblocked(grid, dest[0], dest[1]):
            print("Source or the destination is blocked")
            return

        # Check if we are already at the destination
        if is_destination(src[0], src[1], dest):
            print("We are already at the destination")
            return

        # Initialize the closed list (visited cells)
        closed_list = [[False for _ in range(COL)] for _ in range(ROW)]
        # Initialize the details of each cell
        cell_details = [[Cell() for _ in range(COL)] for _ in range(ROW)]

        # Initialize the start cell details
        i = src[0]
        j = src[1]
        cell_details[i][j].f = 0
        cell_details[i][j].g = 0
        cell_details[i][j].h = 0
        cell_details[i][j].parent_i = i
        cell_details[i][j].parent_j = j

        # Initialize the open list (cells to be visited) with the start cell
        open_list = []
        heapq.heappush(open_list, (0.0, i, j))

        # Initialize the flag for whether destination is found
        found_dest = False

        # Main loop of A* search algorithm
        while len(open_list) > 0:
            # Pop the cell with the smallest f value from the open list
            p = heapq.heappop(open_list)

            # Mark the cell as visited
            i = p[1]
            j = p[2]
            closed_list[i][j] = True

            # For each direction, check the successors
            directions = [(0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1)]
            for dir in directions:
                new_i = i + dir[0]
                new_j = j + dir[1]

                # If the successor is valid, unblocked, and not visited
                if is_valid(new_i, new_j) and is_unblocked(grid, new_i, new_j) and not closed_list[new_i][new_j]:
                    # If the successor is the destination
                    if is_destination(new_i, new_j, dest):
                        # Set the parent of the destination cell
                        cell_details[new_i][new_j].parent_i = i
                        cell_details[new_i][new_j].parent_j = j
                        print("The destination cell is found")
                        # Trace and print the path from source to destination
                        returnedAStarPath = trace_path(cell_details, dest)
                        found_dest = True
                        return returnedAStarPath
                    else:
                        # Calculate the new f, g, and h values
                        g_new = cell_details[i][j].g + 1.0
                        h_new = calculate_h_value(new_i, new_j, dest)
                        f_new = g_new + h_new

                        # If the cell is not in the open list or the new f value is smaller
                        if cell_details[new_i][new_j].f == float('inf') or cell_details[new_i][new_j].f > f_new:
                            # Add the cell to the open list
                            heapq.heappush(open_list, (f_new, new_i, new_j))
                            # Update the cell details
                            cell_details[new_i][new_j].f = f_new
                            cell_details[new_i][new_j].g = g_new
                            cell_details[new_i][new_j].h = h_new
                            cell_details[new_i][new_j].parent_i = i
                            cell_details[new_i][new_j].parent_j = j

        # If the destination is not found after visiting all cells
        if not found_dest:
            print("Failed to find the destination cell")

    # %% [markdown]
    # 1.) Randomly select a coordinate as the start and a coordinate as the end.
    # 
    # 2.) Randomly select a destination coordinate and run the algorithm on it.
    # 
    # 3.) Remove the start coordinate and all coordinates that the path visited.
    # 
    # 4.) Randomly select a remaining coordinate and repeat all steps until none are left.

    # %%
    otherNumberToSet = 0
    result_array_2d = [[otherNumberToSet for _ in range(cols)] for _ in range(rows)]

    lengthOfReturnedSetOfCoordinates = len(returnedSetOfCoordinates)

    for currentPosition in range(lengthOfReturnedSetOfCoordinates):
        if (currentPosition > 0):

            startingCoordinate = returnedSetOfCoordinates[currentPosition - 1]
            endingCoordinate = returnedSetOfCoordinates[currentPosition]

            try:
                returnedAStarPath = a_star_search(array_2d, startingCoordinate, endingCoordinate)

                for coordinate in returnedAStarPath:
                    currentXCoordinate = coordinate[0]
                    currentYCoordinate = coordinate[1]
                    result_array_2d[currentXCoordinate][currentYCoordinate] = 1
                

                # Print the entire array without truncation
                np.set_printoptions(threshold=np.inf, linewidth=500)

                print(np.matrix(result_array_2d))

                # Restore default print options
                np.set_printoptions(threshold=1000, linewidth=75)

                print("\n\n\n")
                
            except:
                pass

    # %% [markdown]
    # Widen paths.
    # 
    # Assisted with the following Google AI search: "python 2d array set box of elements around coordinate to 1"
    # 
    # 

    # %%
    def set_box(array_2d, center_row, center_col, box_size):
        """
        Sets a box of elements around a coordinate in a 2D array to 1.

        Args:
            array_2d: The 2D array to modify.
            center_row: The row index of the center of the box.
            center_col: The column index of the center of the box.
            box_size: The size of the box (number of elements in each dimension).
        """
        rows = len(array_2d)
        cols = len(array_2d[0])
        half_box = box_size // 2

        for r in range(center_row - half_box, center_row + half_box + 1):
            for c in range(center_col - half_box, center_col + half_box + 1):
                if 0 <= r < rows and 0 <= c < cols:
                    array_2d[r][c] = 1

    # %% [markdown]
    # Widen paths.

    # %%
    coordinatesOfFloorTiles = np.argwhere(np.matrix(result_array_2d) == 1)
    print(coordinatesOfFloorTiles)


    for currentCoordinate in coordinatesOfFloorTiles:
        center_row = currentCoordinate[0]
        center_col = currentCoordinate[1]
        set_box(result_array_2d, center_row, center_col, PATH_WIDTH)


    # Print the entire array without truncation
    np.set_printoptions(threshold=np.inf, linewidth=500)

    print(np.matrix(result_array_2d))

    # Restore default print options
    np.set_printoptions(threshold=1000, linewidth=75)

    print("\n\n\n")

    # %% [markdown]
    # Generate rooms. This includes the start room, the tower room, and other various rooms.

    # %%
    # Find Euclidian distance between the center coordinate and a given coordinate.
    def findEuclidianDistance(centerCoordinate, returnedSetOfCoordinatesCopy, currentCoordinatePosition):
        potentialFarthestCoordinate = np.array(returnedSetOfCoordinatesCopy[currentCoordinatePosition])

        # Find the sum of squares.
        sumOfSquares = np.sum(np.square(centerCoordinate - potentialFarthestCoordinate))

        # Do square root to find Euclidan distance.
        squareRootResult = np.sqrt(sumOfSquares)

        return squareRootResult, potentialFarthestCoordinate


    # Copy of returned set of coordinates list.
    returnedSetOfCoordinatesCopy = returnedSetOfCoordinates

    # Use Euclidian distance to find the farthest two coodinates from the center coordinate.
    centerCoordinate = np.array((math.ceil(MAX_RANGE / 2), math.ceil(MAX_RANGE / 2)))
    maxEuclidianDistance = -1
    secondMaxEuclidianDistance = -1

    # The tower coordinate is the farthest coordinate.
    towerCenterRow = -1
    towerCenterCol = -1
    towerCoordinatePosition = -1

    # The start coordinate is the second farthest coordinate.
    startCenterRow = -1
    startCenterCol = -1
    startCoordinatePosition = -1


    # Find the tower coordinates.
    for currentCoordinatePosition in range(len(returnedSetOfCoordinatesCopy)):
        # Find the Euclidian distance and the coordinate.
        squareRootResult, potentialFarthestCoordinate = findEuclidianDistance(centerCoordinate, returnedSetOfCoordinatesCopy, currentCoordinatePosition)

        # Set the tower coordinates.
        if (maxEuclidianDistance < squareRootResult):
            maxEuclidianDistance = squareRootResult
            towerCenterRow = potentialFarthestCoordinate[0]
            towerCenterCol = potentialFarthestCoordinate[1]
            towerCoordinatePosition = currentCoordinatePosition

    # Set tower coordinates in 2D arrays.
    returnedSetOfCoordinatesCopy.pop(towerCoordinatePosition)
    coordinate_only_array_2d[towerCenterRow][towerCenterCol] = 2


    # Find the start coordinates.
    for currentCoordinatePosition in range(len(returnedSetOfCoordinatesCopy)):
        # Find the Euclidian distance and the coordinate.
        squareRootResult, potentialFarthestCoordinate = findEuclidianDistance(centerCoordinate, returnedSetOfCoordinatesCopy, currentCoordinatePosition)

        # Set the start coordinates.
        if (secondMaxEuclidianDistance < squareRootResult):
            secondMaxEuclidianDistance = squareRootResult
            startCenterRow = potentialFarthestCoordinate[0]
            startCenterCol = potentialFarthestCoordinate[1]
            startCoordinatePosition = currentCoordinatePosition

    # Set start coordinates in 2D arrays.
    if(GENERATE_SPAWN_BOX == 1):
        returnedSetOfCoordinatesCopy.pop(startCoordinatePosition)
        coordinate_only_array_2d[startCenterRow][startCenterCol] = 4


    # Print the entire array without truncation
    np.set_printoptions(threshold=np.inf, linewidth=500)

    print(np.matrix(result_array_2d))
    print()
    print()
    print(np.matrix(coordinate_only_array_2d))

    # Restore default print options
    np.set_printoptions(threshold=1000, linewidth=75)

    print("\n\n\n")

    # %% [markdown]
    # Make grid symmetrical by flipping the existing filled-in portion.

    # %%
    additionalCoordinates = []
    friendlyTowerCoordinates = (towerCenterRow, towerCenterCol) # 2
    friendlyStartCoordinates = (startCenterRow, startCenterCol) # 4

    flipped_coordinate_only_array_2d = [[0 for _ in range(cols)] for _ in range(rows)]


    def markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates):
        for currentRowPosition in range(len(intermediary_matrix)):
            for currentColumnPosition in range(len(intermediary_matrix)):
                # Add normal tiles.
                if (intermediary_matrix[currentRowPosition][currentColumnPosition] == 1):
                    result_array_2d[currentRowPosition][currentColumnPosition] = 1
        
        return (result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)


    print(RANDOM_MIRROR_CHOICE)

    if (RANDOM_MIRROR_CHOICE == 0 or RANDOM_MIRROR_CHOICE == 1):
        intermediary_matrix = np.fliplr(result_array_2d)
        intermediary_matrix = intermediary_matrix.tolist()
        returnedFriendlyResults = markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)
        result_array_2d = returnedFriendlyResults[0]
        friendlyTowerCoordinates = returnedFriendlyResults[1]
        friendlyStartCoordinates = returnedFriendlyResults[2]

        flipped_coordinate_only_array_2d = np.fliplr(coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = flipped_coordinate_only_array_2d.tolist()


    elif (RANDOM_MIRROR_CHOICE == 2 or RANDOM_MIRROR_CHOICE == 3):
        # leftHalfCheck and rightHalfCheck
        intermediary_matrix = np.flipud(result_array_2d)
        intermediary_matrix = intermediary_matrix.tolist()
        returnedFriendlyResults = markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)
        result_array_2d = returnedFriendlyResults[0]
        friendlyTowerCoordinates = returnedFriendlyResults[1]
        friendlyStartCoordinates = returnedFriendlyResults[2]

        flipped_coordinate_only_array_2d = np.flipud(coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = flipped_coordinate_only_array_2d.tolist()


    elif (RANDOM_MIRROR_CHOICE == 4):
        # diagonalTopLeftCheck
        intermediary_matrix = np.fliplr(result_array_2d)
        intermediary_matrix = np.rot90(intermediary_matrix, k = -1)
        intermediary_matrix = intermediary_matrix.tolist()
        returnedFriendlyResults = markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)
        result_array_2d = returnedFriendlyResults[0]
        friendlyTowerCoordinates = returnedFriendlyResults[1]
        friendlyStartCoordinates = returnedFriendlyResults[2]

        flipped_coordinate_only_array_2d = np.fliplr(coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = np.rot90(flipped_coordinate_only_array_2d, k = -1)
        flipped_coordinate_only_array_2d = flipped_coordinate_only_array_2d.tolist()


    elif (RANDOM_MIRROR_CHOICE == 5):
        # diagonalTopRightCheck
        intermediary_matrix = np.fliplr(result_array_2d)
        intermediary_matrix = np.rot90(intermediary_matrix)
        intermediary_matrix = intermediary_matrix.tolist()
        returnedFriendlyResults = markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)
        result_array_2d = returnedFriendlyResults[0]
        friendlyTowerCoordinates = returnedFriendlyResults[1]
        friendlyStartCoordinates = returnedFriendlyResults[2]

        flipped_coordinate_only_array_2d = np.fliplr(coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = np.rot90(flipped_coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = flipped_coordinate_only_array_2d.tolist()


    elif (RANDOM_MIRROR_CHOICE == 6):
        # diagonalBottomRightCheck
        intermediary_matrix = np.fliplr(result_array_2d)
        intermediary_matrix = np.rot90(intermediary_matrix, k = -1)
        intermediary_matrix = intermediary_matrix.tolist()
        returnedFriendlyResults = markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)
        result_array_2d = returnedFriendlyResults[0]
        friendlyTowerCoordinates = returnedFriendlyResults[1]
        friendlyStartCoordinates = returnedFriendlyResults[2]

        flipped_coordinate_only_array_2d = np.fliplr(coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = np.rot90(flipped_coordinate_only_array_2d, k = -1)
        flipped_coordinate_only_array_2d = flipped_coordinate_only_array_2d.tolist()


    elif (RANDOM_MIRROR_CHOICE == 7):
        # diagonalBottomLeftCheck
        intermediary_matrix = np.fliplr(result_array_2d)
        intermediary_matrix = np.rot90(intermediary_matrix)
        intermediary_matrix = intermediary_matrix.tolist()
        returnedFriendlyResults = markFlippedAndOrRotatedGridPieces(intermediary_matrix, result_array_2d, friendlyTowerCoordinates, friendlyStartCoordinates)
        result_array_2d = returnedFriendlyResults[0]
        friendlyTowerCoordinates = returnedFriendlyResults[1]
        friendlyStartCoordinates = returnedFriendlyResults[2]

        flipped_coordinate_only_array_2d = np.fliplr(coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = np.rot90(flipped_coordinate_only_array_2d)
        flipped_coordinate_only_array_2d = flipped_coordinate_only_array_2d.tolist()
        

    else:
        raise Exception("Sorry, 'RANDOM_MIRROR_CHOICE' must be between 0 and 7, inclusive.")

    # %% [markdown]
    # Create rooms.

    # %%
    for currentRowPosition in range(len(result_array_2d)):
        for currentColumnPosition in range(len(result_array_2d)):
            # Generate intermediary rooms.
            if ((coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 1) or (flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 1)):
                set_box(result_array_2d, currentRowPosition, currentColumnPosition, INTERMEDIARY_BOX_SIZE)
            
            # Generate tower rooms.
            if ((coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 2) or (flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 2)):
                set_box(result_array_2d, currentRowPosition, currentColumnPosition, TOWER_BOX_SIZE)
            
            # Generate start rooms.
            if ((coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 4) or (flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 4)):
                set_box(result_array_2d, currentRowPosition, currentColumnPosition, SPAWN_BOX_SIZE)

    # %% [markdown]
    # Make the pixel image array with the 2D array.

    # %%
    import cv2

    enemyTowerCoordinates = []
    enemyStartCoordinates = []

    towerCoordinatesOverridden = False
    startCoordinatesOverridden = False
    coordinateInWall = False

    onlyCenterCoordinateEdgeCaseCount = 0

    colorsToCheck = ([0, 128, 128], [255, 255, 0], [0, 0, 233], [255, 165, 0], [0, 255, 0], [255, 0, 0])

    # Create a Numpy array
    length_of_result_array_2d = len(result_array_2d)
    numpy_result_array_2d = np.array(result_array_2d)
    color2DArray = np.zeros((numpy_result_array_2d.shape[0], numpy_result_array_2d.shape[1], 3), dtype=np.uint8)

    def generateBorderWallOrCheckBorderOverlap(generateOrCheck, rgbColorToUseOrCheck, currentCheckValue, currentRowPosition, currentColumnPosition):
        if ((currentRowPosition == 0) or (currentRowPosition == (length_of_result_array_2d - 1)) or 
            (currentColumnPosition == 0) or (currentColumnPosition == (length_of_result_array_2d - 1))):
            if (generateOrCheck):
                color2DArray[currentRowPosition, currentColumnPosition] = rgbColorToUseOrCheck
            else:
                if ((color2DArray[currentRowPosition, currentColumnPosition] == rgbColorToUseOrCheck).all()):
                    return True
        return currentCheckValue

    # Mark all friendly and enemy locations.
    for currentRowPosition in range(numpy_result_array_2d.shape[0]):
        for currentColumnPosition in range(numpy_result_array_2d.shape[1]):

            # Walls are black.
            if (numpy_result_array_2d[currentRowPosition][currentColumnPosition] == 0):
                color2DArray[currentRowPosition, currentColumnPosition] = [0, 0, 0]

            # Tiles are white.
            if (numpy_result_array_2d[currentRowPosition][currentColumnPosition] == 1):
                color2DArray[currentRowPosition, currentColumnPosition] = [255, 255, 255]
            
            # Generate border wall.
            coordinateInWall = generateBorderWallOrCheckBorderOverlap(True, [0, 0, 0], coordinateInWall, currentRowPosition, currentColumnPosition)

            # Friendly intermediary coordinates are teal.
            if (coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 1):
                color2DArray[currentRowPosition, currentColumnPosition] = [0, 128, 128]
                onlyCenterCoordinateEdgeCaseCount += 1

            # Enemy intermediary coordinates are yellow.
            if (flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 1):
                color2DArray[currentRowPosition, currentColumnPosition] = [255, 255, 0]
                onlyCenterCoordinateEdgeCaseCount += 1

                # Overlapping intermediary coordinates are grey.
                if(coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition]):
                    color2DArray[currentRowPosition, currentColumnPosition] = [192, 192, 192]

            # Friendly tower is dark blue.
            if (coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 2):
                color2DArray[currentRowPosition, currentColumnPosition] = [0, 0, 233]

            # Enemy tower is orange.
            if (flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 2):
                color2DArray[currentRowPosition, currentColumnPosition] = [255, 165, 0]
                enemyTowerCoordinates.append((currentRowPosition, currentColumnPosition))

                if(coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition]):
                    towerCoordinatesOverridden = True

            # Friendly start is green.
            if (coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 4):
                color2DArray[currentRowPosition, currentColumnPosition] = [0, 255, 0]

            # Enemy start is red.
            if (flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == 4):
                color2DArray[currentRowPosition, currentColumnPosition] = [255, 0, 0]
                enemyStartCoordinates.append((currentRowPosition, currentColumnPosition))

                if(coordinate_only_array_2d[currentRowPosition][currentColumnPosition] == flipped_coordinate_only_array_2d[currentRowPosition][currentColumnPosition]):
                    startCoordinatesOverridden = True
            
            # Check if coordinate is on border.
            for element in colorsToCheck:
                coordinateInWall = generateBorderWallOrCheckBorderOverlap(False, element, coordinateInWall, currentRowPosition, currentColumnPosition)

    # %% [markdown]
    # Rerun Python program if:
    # 1.) The friendly and enemy towers / spawns are too close.
    # 2.) The enemy tower or enemy spawns weren't created.
    # 3.) One corrdinate overrode another.
    # 4.) The edge case appeared where only 1 center coordinate is created.
    # 
    # Otherwise, create the image.

    safeToDrawImage = True

    # Check for the edge case were only 1 center coordinate is created.
    print(onlyCenterCoordinateEdgeCaseCount)
    if (onlyCenterCoordinateEdgeCaseCount <= 1):
        safeToDrawImage = False
        rerunEverythingCheck = True

    # Check if one coordinate accidentally overrode another.
    if((towerCoordinatesOverridden == True) or (startCoordinatesOverridden == True)):
        safeToDrawImage = False
        rerunEverythingCheck = True

    # Check if the enemy tower coordinates were never created.
    if (enemyTowerCoordinates == []):
        safeToDrawImage = False
        rerunEverythingCheck = True
    else:
        # Check if the friendly and enemy tower coordinates are too close.
        friendlyTowerCoordinates = (np.int64(friendlyTowerCoordinates[0]), np.int64(friendlyTowerCoordinates[1]))
        distanceBetweenFriendlyAndEnemyTowers = findEuclidianDistance(friendlyTowerCoordinates, enemyTowerCoordinates, 0)[0]

        if ((distanceBetweenFriendlyAndEnemyTowers <= TOWER_BOX_SIZE + 2)):
            safeToDrawImage = False
            rerunEverythingCheck = True

    # Check if the enemy start coordinates were never created.
    if ((GENERATE_SPAWN_BOX == 1) and (enemyStartCoordinates == [])):
        safeToDrawImage = False
        rerunEverythingCheck = True
    else:
        # Check if the friendly and enemy start coordinates are too close.
        if (GENERATE_SPAWN_BOX == 1):
            friendlyStartCoordinates = (friendlyStartCoordinates[0].item(), friendlyStartCoordinates[1].item())
            distanceBetweenFriendlyAndEnemyStarts = findEuclidianDistance(friendlyStartCoordinates, enemyStartCoordinates, 0)[0]

            if ((distanceBetweenFriendlyAndEnemyStarts <= TOWER_BOX_SIZE + 2)):
                safeToDrawImage = False
                rerunEverythingCheck = True

    # Check if coordinate is in border wall.
    if (coordinateInWall):
        safeToDrawImage = False
        rerunEverythingCheck = True


    if (safeToDrawImage == True):
        # Convert array to image.
        bgr_image = cv2.cvtColor(color2DArray, cv2.COLOR_RGB2BGR)
        cv2.imwrite('output.png', bgr_image)


