/*
 * Conway's Game of Life Simulation
 * 
 * The simulation is based on the following rules:
 * 1) Any live cell with fewer than two live neighbours dies, as if caused by under-population.
 * 2) Any live cell with two or three live neighbours lives on to the next generation.
 * 3) Any live cell with more than three live neighbours dies, as if by overcrowding.
 * 4) Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
 * 
 * Program.cs - Only using one file here to make sharing easier.
 *              File contains classes for cells, game grid, etc.
 * 
 * Author(s): Joshua Mitchell
 * 2013
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    #region Cell Class

    /// <summary>
    /// This class represents a single cell in the simulation.
    /// Either alive or dead.
    /// </summary>
    class Cell 
    {
        /// <summary>
        /// Enum for defining state of the cell
        /// </summary>
        public enum State
        {
            DEAD, ALIVE,
        }

        // State variable with getters and setters
        public State mState, mNextState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state"> Cell starts either DEAD or ALIVE </param>
        public Cell(State state)
        {
            mState = state;
            mNextState = state;
        }

        /// <summary>
        /// Default constructor. Cell starts DEAD
        /// </summary>
        public Cell()
        {
            mState = State.DEAD;
            mNextState = State.DEAD;
        }

        /// <summary>
        /// Updates the cells current state
        /// </summary>
        public void Update()
        {
            mState = mNextState;
        }

        /// <summary>
        /// "Draw" function for drawing the simulation to the console.
        /// </summary>
        /// <returns> A string representation of the state </returns>
        public void Draw(){
            if (mState == State.ALIVE)
            {
                Console.Write("X");
            }
            else
            {
                Console.Write(" ");
            }
        }
    }

    #endregion

    #region Grid Class

    /// <summary>
    /// Grid where the cells reside 
    /// 2D array representation
    /// </summary>
    class Grid
    {
        /// <summary>
        /// 2D array of cells for the grid
        /// </summary>
        public Cell[,] mCells;

        /// <summary>
        /// Variables for defining the size of our grid
        /// </summary>
        public int mWidth { get; private set; }
        public int mHeight { get; private set; }

        /// <summary>
        /// Grid constructor
        /// </summary>
        /// <param name="width"> width of grid </param>
        /// <param name="height"> height of grid</param>
        public Grid(int width, int height)
        {
            mWidth = width;
            mHeight = height;

            //init the grid array
            mCells = new Cell[mWidth, mHeight];

            //fill it with cells
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    mCells[i, j] = new Cell();
                }
            }

        }

        /// <summary>
        /// Grid constructor
        /// </summary>
        /// <param name="width"> width of grid </param>
        /// <param name="height"> height of grid</param>
        public Grid(int[,] cellGrid)
        {
            mWidth = cellGrid.GetLength(0);
            mHeight = cellGrid.GetLength(1);

            //init the grid array
            mCells = new Cell[mWidth, mHeight];

            //fill it with cells
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    mCells[i, j] = new Cell( cellGrid[i, j] == 1 ? Cell.State.ALIVE : Cell.State.DEAD);
                }
            }

        }

        /// <summary>
        /// Function that applies the rules of the simulation to the cell located in the grid at [x, y]
        /// </summary>
        /// <param name="row"> row index </param>
        /// <param name="col"> column index</param>
        private void ApplyRulesToCell(int row, int col)
        {
            //is the cell alive this generation
            bool alive = (mCells[row, col].mState == Cell.State.ALIVE);
            bool nextState = false;     //next generation

            //find out how many of it's neighbors are alive
            int count = getNumberOfLivingNeighbors(row, col);

            //Run through the rules in order
            //1) Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            if (alive && count < 2)
            {
                nextState = false;
            }
            //2) Any live cell with two or three live neighbours lives on to the next generation.
            if (alive && (count == 2 || count == 3))
            {
                nextState = true;
            }
            //3) Any live cell with more than three live neighbours dies, as if by overcrowding.
            if (alive && count > 3)
            {
                nextState = false;
            }
            //4) Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            if (!alive && count == 3)
            {
                nextState = true;
            }

            //set dead or alive state for the next generation of this cell
            mCells[row, col].mNextState = nextState ? Cell.State.ALIVE : Cell.State.DEAD;
        }

        /// <summary>
        /// Function that checks the neighbors of a cell
        /// </summary>
        /// <param name="row"> row index of cell </param>
        /// <param name="col"> column index of cell </param>
        /// <returns> number of neighbors that are alive </returns>
        private int getNumberOfLivingNeighbors(int row, int col)
        {
            int result = 0;

            //top left neighbor
            if (row != 0 && col != 0)
            {
                if (mCells[row - 1, col - 1].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //top neighbor
            if (col != 0)
            {
                if (mCells[row, col - 1].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //top right neighbor
            if (row != mWidth - 1 && col != 0)
            {
                if (mCells[row + 1, col - 1].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //bottom left neighbor
            if (row != 0 && col != mHeight - 1)
            {
                if (mCells[row - 1, col + 1].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //bottom neighbor
            if (col != mHeight - 1)
            {
                if (mCells[row, col + 1].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //bottom right neighbor
            if (row != mWidth - 1 && col != mHeight - 1)
            {
                if (mCells[row + 1, col + 1].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //left neighbor
            if (row != 0)
            {
                if (mCells[row - 1, col].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            //right neighbor
            if (row != mWidth - 1)
            {
                if (mCells[row + 1, col].mState == Cell.State.ALIVE)
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Update the cells of the grid
        /// </summary>
        public void Update()
        {
            //loop through all the cells to apply simulation rules
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    ApplyRulesToCell(i, j);
                }
            }

            //loop through again to update cell state
            //this looks like duplicate code, but it's necessary for the integrity of the simulation
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    mCells[i, j].Update();
                }
            }
        }

        /// <summary>
        /// Draw up the grid in the console
        /// </summary>
        public void Draw()
        {
            //loop through all the cells and tell them to draw themselves
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    mCells[i, j].Draw();
                }
                Console.WriteLine();
            }
        }
    }

    #endregion


    class Program 
    {
        static void Main(string[] args) 
        {
            //TODO: Tuneable grid size with random seeds

            //tuneable amount of time for each generation to last (stay in console)
            int generationLifeTime = 400;

            #region Simulation Seed

            //Set up a seed grid of ints
            int[,] seed = new int[,]
	        {
                //This is a cool seed
                /*
		        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                */

                //This is cool too!
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0,},
	        };

            #endregion

            //create the actual grid
            Grid GameOfLife = new Grid(seed);

            //initial draw
            GameOfLife.Draw();

            //prompt for the okay to begin simulation
            Console.WriteLine("\n\nPress any key to begin the game of life \nbeginning with generation 1, displayed above...");
            Console.ReadKey();

            //Run simulation 
            while (true)
            {
                //clear the console
                Console.Clear();

                //update and draw current generation
                GameOfLife.Update();
                GameOfLife.Draw();

                //Sleep the thread for a fraction of a second
                System.Threading.Thread.Sleep(generationLifeTime);
            }
        }
    }
}
