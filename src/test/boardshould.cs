using System;
using System.Diagnostics;
using soduku;
using Xunit;

namespace test
{
    public class BoardShould
    {
        [Fact]
        public void InitializeProperly()
        {
            Board board = new Board();

            int[,] cells = board.Cells;

            Assert.Equal(9, cells.GetLength(0));
            Assert.Equal(9, cells.GetLength(1));

            for (int r = 0; r < 9; ++r)
                for (int c = 0; c < 9; ++c)
                    Assert.Equal(0, cells[r, c]);

            Assert.False(board.IsValid());
        }

        [Fact]
        public void PopulateProperly()
        {
            Board board = new Board();
            bool success = board.Populate();

            Assert.True(success);

            int[,] cells = board.Cells;

            for (int r = 0; r < 9; ++r)
                for (int c = 0; c < 9; ++c)
                    Assert.NotEqual(0, cells[r, c]);
        }

        [Fact]
        public void TakeLessThan1msToPopulate()
        {
            Board board = new Board();
            Stopwatch watch = Stopwatch.StartNew();

            for (int i = 0; i < 100; ++i)
                board.Populate();

            long elapsed = watch.ElapsedMilliseconds;

            Console.WriteLine($"{elapsed} ms");
            Assert.True(elapsed < 100);
        }
    }
}
