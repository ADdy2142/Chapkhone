using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.Utilities.Pagination
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }
        public bool PreviousPage => PageIndex > 1;
        public bool NextPage => PageIndex < TotalPages;
        public bool HasAroundPages => TotalPages > 6;
        public int[] AroundPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (HasAroundPages)
                AroundPages = GetAroundPages();

            this.AddRange(items);
        }

        private int[] GetAroundPages()
        {
            var result = new int[5];
            if (PageIndex < 4)
            {
                result[PageIndex - 1] = PageIndex;

                for (int i = 0; i < PageIndex - 1; i++)
                    result[i] = i + 1;

                for (int i = PageIndex; i < 5; i++)
                    result[i] = i + 1;
            }
            else if (TotalPages - PageIndex < 3)
            {
                int totalIndices = result.Length - 1;
                int startIndex = totalIndices - (TotalPages - PageIndex);
                result[startIndex] = PageIndex;

                for (int i = 0; i < startIndex; i++)
                    result[i] = PageIndex - startIndex + i;

                for (int i = startIndex + 1; i < 5; i++)
                    result[i] = PageIndex - startIndex + i;
            }
            else
            {
                result[2] = PageIndex;

                int temp = 2;
                for (int i = 0; i < 2; i++)
                {
                    result[i] = PageIndex - temp;
                    temp--;
                }

                temp = 1;
                for (int i = 3; i < 5; i++)
                {
                    result[i] = PageIndex + temp;
                    temp++;
                }
            }

            return result;
        }

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}