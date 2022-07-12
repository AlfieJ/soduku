using System;
using System.Collections.Generic;

namespace soduku
{
    /// <summary>
    /// Base class for blocks, rows, and columns
    /// </summary>
    internal class Base
    {
        /// <summary>
        /// Each block, row, and column references the same set of
        /// cells.
        /// </summary>
        protected readonly List<int> _cells;

        /// <summary>
        /// Each Block/Row/Column will have a different subset of the cells
        /// they sit over. We calculate the indices into the _cells list
        /// once at creation time.
        /// </summary>
        public List<int> Indices{ get; private set; }

        /// <summary>
        /// Return a list of numbers that are allowed in this block/row/column
        /// Once a number is used in this block/row/column, it is no longer allowed.
        /// </summary>
        public List<int> Allowed
        {
            get
            {
                List<int> allowed = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Indices.ForEach(i =>
                {
                    int val = _cells[i];
                    if (val > 0)
                        allowed.Remove(val);
                });
                return allowed;
            }
        }

        public Base(List<int> cells)
        {
            _cells = cells;
            Indices = new List<int>();
        }

        /// <summary>
        /// Used to see if every cell is filled with a valid number and all 9 numbers are used
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            bool[] valid = new bool[9];
            for (int i = 0; i < 9; ++i)
            {
                int value = _cells[Indices[i]];
                bool isValid = value.IsValid();
                if(!isValid)
                    return false;
                valid[value - 1] = isValid;
            }
            return Array.TrueForAll(valid, v => v);
        }
    }

    internal class Block : Base
    {
        public (int, int) BlockNum{ get; private set; }

        public Block((int, int) num, List<int> cells) : base(cells)
        {
            BlockNum = num;

            int startCell = (num.Item1 * (9 * 3)) + (num.Item2 * 3);
            for (int r = 0; r < 3; ++r)
            {
                Indices.AddRange(new int[] { startCell, startCell + 1, startCell + 2 });
                startCell += 9;
            }
        }
    }

    internal class Column : Base
    {
        public int ColumnNum{ get; private set; }

        public Column(int col, List<int> cells) : base(cells)
        {
            ColumnNum = col;
            for (int i = 0; i < cells.Count; ++i)
            {
                if(i % 9 == col)
                    Indices.Add(i);
            }
        }
    }

    internal class Row : Base
    {
        public int RowNum{ get; private set; }

        public Row(int row, List<int> cells) : base(cells)
        {
            RowNum = row;

            for (int i = 0; i < 9; ++i)
                Indices.Add(row * 9 + i);
        }

        public override string ToString()
        {
            string str = string.Empty;
            for (int i = 0; i < Indices.Count; ++i)
            {
                if (i > 0 && i % 3 == 0)
                    str += "| ";
                str += $"{_cells[Indices[i]]} ";
            }
            return str;
        }
    }
}