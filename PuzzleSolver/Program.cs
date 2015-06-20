using System;
using System.Collections.Generic;
using System.Text;

namespace PuzzleSolver
{
    class Program
    {
        private static char[,] m_puzzle = null; // 2-d array
        private static List<Tuple<int, int>> m_arrayIndexTuple = null; // coordinate of all indexes in m_puzzle
        private static int m_rows, m_cols;
        private static HashSet<Tuple<int, int>> m_alreadyFoundSet = null; // keep track of the location of 1 letter words

        private enum eDirection
        {
            HORIZONTAL,
            VERTICAL,
            HORIZONTAL_REVERSE,
            VERTICAL_REVERSE,
            DIAGONAL_SE,
            DIAGONAL_SW,
            DIAGONAL_NE,
            DIAGONAL_NW
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Start!");

            //m_puzzle = new char[3, 3] // 8 words
            //{
            //    {'C', 'A', 'T'},
            //    {'X', 'Z', 'T'},
            //    {'Y', 'O', 'T'}
            //};

            m_puzzle = new char[3, 8] // 22 words
            {
                {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T'},
                {'X', 'Z', 'T', 'T', 'O', 'Y', 'O', 'O'},
                {'Y', 'O', 'T', 'O', 'X', 'T', 'X', 'X'}
            };

            //m_puzzle = new char[9, 9] 
            //{
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'},
            //    {'X', 'Z', 'T', 'T', 'O', 'Y', 'O', 'O', 'Z'},
            //    {'Y', 'O', 'T', 'O', 'X', 'T', 'X', 'X', 'T'},
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'},
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'},
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'},
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'},
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'},
            //    {'C', 'A', 'T', 'A', 'P', 'U', 'L', 'T', 'X'}
            //};

            _Init();//Initialize
            Console.WriteLine("Finding words...");
            Console.WriteLine("Found {0} word", FindWords(m_puzzle));
        }


        /// <summary>
        /// return the number of all non-distinct occurrences
        /// </summary>
        /// <param name="puzzle"></param>
        /// <returns>number of words found in array</returns>
        public static int FindWords(char[,] puzzle)
        {
            int count = 0;
            _SetArrayIndexPairs();//build array index

            //foreach array index build horizontal, vertical, and diagonal links in all directions
            foreach(Tuple<int, int> currentIndex in m_arrayIndexTuple)
            {
                //build horizontal links for current index
                List<Tuple<int, int>> horizontal = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.HORIZONTAL);
                
                //build vertical links for current index
                List<Tuple<int, int>> vertical = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.VERTICAL);

                //build horizontal reverse links for current index
                List<Tuple<int, int>> horizontal_reverse = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.HORIZONTAL_REVERSE);

                //build vertical reverse links for current index
                List<Tuple<int, int>> vertical_reverse = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.VERTICAL_REVERSE);

                //build diagonal south east links for current index
                List<Tuple<int, int>> diagonal_se = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.DIAGONAL_SE);

                //build diagonal north west links for current index
                List<Tuple<int, int>> diagonal_nw = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.DIAGONAL_NW);

                //build diagonal south west links for current index
                List<Tuple<int, int>> diagonal_sw = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.DIAGONAL_SW);

                //build diagonal north east links for current index
                List<Tuple<int, int>> diagonal_ne = _BuildLinksInDirection(currentIndex.Item1, currentIndex.Item2, eDirection.DIAGONAL_NE);

                //process horizontal 
                count += _Process(horizontal);

                //process horizontal reverse
                count += _Process(horizontal_reverse);

                //process vertical
                count += _Process(vertical);

                //process vertical reverse
                count += _Process(vertical_reverse);

                //process diagonal south east
                count += _Process(diagonal_se);

                //process diagonal north west
                count += _Process(diagonal_nw);

                //process diagonal south west
                count += _Process(diagonal_sw);

                //process diagonal north east
                count += _Process(diagonal_ne);
            }

            return count;
        }

        /// <summary>
        /// Init
        /// </summary>
        private static void _Init()
        {
            m_alreadyFoundSet = new HashSet<Tuple<int, int>>();

            try
            {
                m_rows = m_puzzle.GetLength(0);
                m_cols = m_puzzle.GetLength(1);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught {0}", e);
            }
        }

        /// <summary>
        /// Set all array index locations
        /// </summary>
        private static void _SetArrayIndexPairs()
        {
            m_arrayIndexTuple = new List<Tuple<int, int>>();
            
            for (int i = 0; i < m_rows; i++)
            {
                for (int j = 0; j < m_cols; j++)
                {
                    m_arrayIndexTuple.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        /// <summary>
        /// Build the links from starting point with given direction
        /// </summary>
        /// <param name="xPos">starting x position</param>
        /// <param name="yPos">starting y position</param>
        /// <param name="direction">direction</param>
        /// <returns>List of Tuples with array index</returns>
        private static List<Tuple<int,int>> _BuildLinksInDirection(int xPos, int yPos, eDirection direction)
        {
            List<Tuple<int, int>> listToReturn = new List<Tuple<int, int>>();
            switch (direction)
            {
                case eDirection.HORIZONTAL:
                    for (int i = yPos; i < m_cols; i++)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(xPos, i);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.VERTICAL:
                    for (int i = xPos; i < m_rows; i++)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(i, yPos);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.HORIZONTAL_REVERSE:
                    for (int i = yPos; i >= 0; i--)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(xPos, i);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.VERTICAL_REVERSE:
                    for (int i = xPos; i >= 0; i--)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(i, yPos);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.DIAGONAL_SE:
                    for (int i = xPos, j = yPos; i < m_rows; i++, j++)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(i, j);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.DIAGONAL_NW:
                    for (int i = xPos, j = yPos; i >= 0; i--, j--)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(i, j);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.DIAGONAL_SW:
                    for (int i = xPos, j = yPos; j >= 0; i++, j--)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(i, j);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                case eDirection.DIAGONAL_NE:
                    for (int i = xPos, j = yPos; i >= 0; i--, j++)
                    {
                        Tuple<int, int> tupleToFind = new Tuple<int, int>(i, j);
                        if (m_arrayIndexTuple.Contains(tupleToFind))
                        {
                            listToReturn.Add(tupleToFind);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                default:
                    throw new Exception("Default case exception");
            }
            return listToReturn;
        }

        /// <summary>
        /// Process word builder
        /// Use IsWord from PuzzleSolver class
        /// </summary>
        /// <param name="_string"></param>
        /// <param name="tuple"></param>
        /// <returns>number of words found</returns>
        private static int _Process(List<Tuple<int, int>> tupleList)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (Tuple<int, int> tuple in tupleList)
            {
                sb.Append(m_puzzle[tuple.Item1, tuple.Item2]);

                if (PuzzleSolver.IsWord(sb.ToString()) == true)
                {
                    bool result = true;
                    //check to see if the word length is 1
                    //if true check to see if it already exists in hashset
                    if (sb.ToString().Length == 1)
                    {
                        result = _AlreadyFound(tuple);
                    }
                    if(result == true)
                    {
                        //Console.WriteLine("Found word: {0}", sb);
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Check to see if location is already found
        /// If not found add it
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>true if not found, false if found</returns>
        private static bool _AlreadyFound(Tuple<int, int> tuple)
        {
            return m_alreadyFoundSet.Add(tuple);
        }

    }
}
