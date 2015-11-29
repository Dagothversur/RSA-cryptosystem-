namespace MyGenerator
{
    public static class InputChecker
    {
        public static int[] CheckInput(string[] info)
        {
            var results = new int[info.Length];
            for (var i = 0; i < info.Length; i++)
            {
                if (!int.TryParse(info[i], out results[i]))
                {
                    return null;
                }
            }

            return results;
        }

        public static int CheckInput(string info)
        {
            int result;
           
                if (!int.TryParse(info, out result))
                {
                    return -1;
                }


            return result;
        }
    }
}
