using System.Collections.Generic;
using System.Linq;

namespace soduku
{
    /// <summary>
    /// Represents an entire Soduku board.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// All the Block/Row/Column objects will link to this one list
        /// of integers
        /// </summary>
        private readonly List<int> _cells;

        private readonly List<Base> _bases;

        /// <summary>
        /// We keep the rows separate so we can easily grab them when generating
        /// a string representing the entire board.
        /// </summary>
        private readonly List<Row> _rows;

        /// <summary>
        /// For performance we map an index of _cell back to the 3 things
        /// that "cover" the cell: Block/Row/Column. We use this to figure
        /// out what the acceptable values in a give cell are given the
        /// current state of the board.
        /// </summary>
        private readonly Dictionary<int, List<Base>> _cellToBaseMap;

        /// <summary>
        /// Get an 2D array of cells representing the board
        /// </summary>
        public int[,] Cells
        {
            get
            {
                int[,] cells = new int[9, 9];
                for (int r = 0; r < 9; ++r)
                {
                    for (int c = 0; c < 9; ++c)
                        cells[r, c] = _cells[r * 9 + c];
                }
                return cells;
            }
        }

        public Board()
        {
            _cells = new List<int>(9 * 9);
            _bases = new List<Base>(9 * 3);
            _rows = new List<Row>(9);
            _cellToBaseMap = new Dictionary<int, List<Base>>(9 * 9);

            _cells.Fill(0);

            Setup();
        }

        public void Clear()
        {
            _cells.Fill(0);
        }

        public bool IsValid()
        {
            bool isValid = true;
            for (int i = 0; isValid && i < _bases.Count; ++i)
                isValid = _bases[i].IsValid();
            return isValid;
        }

        public bool Populate()
        {
            Clear();
            return Populate(0);
        }

        public override string ToString()
        {
            string str = string.Empty;
            for (int i = 0; i < _rows.Count; ++i)
            {
                if (i > 0 && i % 3 == 0)
                    str += "---------------------\n";
                str += _rows[i].ToString() + "\n";
            }
            return str;
        }

        private void Setup()
        {
            for (int i = 0; i < 9; ++i)
            {
                Row r = new Row(i, _cells);
                Column c = new Column(i, _cells);

                _rows.Add(r);

                _bases.Add(r);
                _bases.Add(c);
            }

            for (int r = 0; r < 3; ++r)
                for (int c = 0; c < 3; ++c)
                {
                    Block b = new Block((r, c), _cells);
                    _bases.Add(b);
                }

            for (int i = 0; i < 81; ++i)
                _cellToBaseMap[i] = new List<Base>(3);
            _bases.ForEach(b =>
                b.Indices.ForEach(i => _cellToBaseMap[i].Add(b)));
        }

        private List<int> AcceptableValues(int index)
        {
            List<Base> bases = _cellToBaseMap[index];

            if(bases.Count > 0)
            {
                List<int> acceptable = bases[0].Allowed;
                for (int i = 1; i < bases.Count; ++i)
                    acceptable = acceptable.Intersect(bases[i].Allowed).ToList();
                return acceptable;
            }

            return new List<int>();
        }

        private bool Populate(int index)
        {
            // If we get all the way through the cells, success!
            if(index >= (9 * 9))
                return true;

            bool success = false;
            List<int> allowable = AcceptableValues(index).Randomize();
            while(success == false && allowable.Count > 0)
            {
                int value = allowable[0];
                allowable.RemoveAt(0);
                _cells[index] = value;

                success = Populate(index + 1);

                if(success == false)
                    _cells[index] = 0;
            }

            return success;
        }
    }
}