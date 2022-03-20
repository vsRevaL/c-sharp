namespace BlazorServer.Data
{
    public class GridBase
    {
        private static int N = 26; // rows
        private static int M = 60; // columns
        public List<List<int>> Grid { get; set; }

        public GridBase()
        {
            Grid = new List<List<int>>();
            for (int i = 0; i < N; i++)
            {
                Grid.Add(new List<int>());
                for (int j = 0; j < M; j++)
                {
                    Grid[i].Add(0);
                }
            }
        }

        public List<List<int>> GetGrid() => Grid;
    }
}
