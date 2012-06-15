using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RGBuild
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 2;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.Ascending;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            int column = ColumnToSort;
            if (column == 2)
                if (listviewX.SubItems[0].Text == "FileSystem" && listviewY.SubItems[0].Text == "FileSystem")
                    column = 1;

            string xToSort = listviewX.SubItems[ColumnToSort].Text.Split(new [] { " " }, StringSplitOptions.None)[0];
            string yToSort = listviewY.SubItems[ColumnToSort].Text.Split(new[] { " " }, StringSplitOptions.None)[0];
            if ((xToSort.Length > 2 && xToSort.Substring(0, 2) == "0x" && yToSort.Length > 2 && yToSort.Substring(0, 2) == "0x") || column != ColumnToSort)
            {
                int ixToSort = int.Parse(xToSort.Substring(2, xToSort.Length - 2), NumberStyles.HexNumber);
                int iyToSort = int.Parse(yToSort.Substring(2, yToSort.Length - 2), NumberStyles.HexNumber);
                compareResult = ObjectCompare.Compare(ixToSort, iyToSort);
            }
            else
            {
                compareResult = ObjectCompare.Compare(xToSort, yToSort);
            }
            if (column == 1)
                compareResult = -compareResult;
            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }
}
