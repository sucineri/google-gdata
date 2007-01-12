/* Copyright (c) 2006 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Xml;
using System.IO;
using System.Collections;
using Google.GData.Client;
using Google.GData.Extensions;

namespace Google.GData.Spreadsheets
{
    /// <summary>
    /// Entry API customization class for defining entries in a Worksheets feed.
    /// </summary>
    public class WorksheetEntry : AtomEntry
    {
        /// <summary>
        /// Category used to label entries that contain Cell extension data.
        /// </summary>
        public static AtomCategory WORKSHEET_CATEGORY
        = new AtomCategory(GDataSpreadsheetsNameTable.Worksheet,
                           new AtomUri(BaseNameTable.gKind));

        private ColCountElement colCount;
        private RowCountElement rowCount;

        /// <summary>
        /// Constructs a new WorksheetEntry instance with the appropriate category
        /// to indicate that it is a worksheet.
        /// </summary>
        public WorksheetEntry() : base()
        {
            Categories.Add(WORKSHEET_CATEGORY);
        }

        /// <summary>
        /// The colCount element in this cell entry
        /// </summary>
        public ColCountElement ColCount
        {
            get { return colCount;}
            set
            {
                if (colCount != null)
                {
                    ExtensionElements.Remove(colCount);
                }
                colCount = value; 
                ExtensionElements.Add(colCount);
            }
        }

        /// <summary>
        /// The rowCount element in this cell entry
        /// </summary>
        public RowCountElement RowCount
        {
            get { return rowCount;}
            set
            {
                if (rowCount != null)
                {
                    ExtensionElements.Remove(rowCount);
                }
                rowCount = value; 
                ExtensionElements.Add(rowCount);
            }
        }

        /// <summary>
        /// Empty base implementation
        /// </summary>
        /// <param name="writer">The XmlWrite, where we want to add default namespaces to</param>
        protected override void AddOtherNamespaces(XmlWriter writer)
        {
            base.AddOtherNamespaces(writer);
            Utilities.EnsureGDataNamespace(writer);
        }

        /// <summary>
        /// Checks if this is a namespace declaration that we already added
        /// </summary>
        /// <param name="node">XmlNode to check</param>
        /// <returns>True if this node should be skipped</returns>
        protected override bool SkipNode(XmlNode node)
        {
            if (base.SkipNode(node))
            {
                return true;
            }

            return(node.NodeType == XmlNodeType.Attribute
                   && node.Name.StartsWith("xmlns")
                   && String.Compare(node.Value, BaseNameTable.gNamespace) == 0);
        }

        /// <summary>
        /// Parses the inner state of the element
        /// </summary>
        /// <param name="worksheetNode">A g-scheme, xml node</param>
        /// <param name="parser">The AtomFeedParser that called this</param>
        public void ParseWorksheet(XmlNode worksheetNode, AtomFeedParser parser)
        {
            if (String.Compare(worksheetNode.NamespaceURI, GDataSpreadsheetsNameTable.NSGSpreadsheets, true) == 0)
            {
                if (worksheetNode.LocalName == GDataSpreadsheetsNameTable.XmlColCountElement)
                {
                    ColCount = ColCountElement.ParseColCount(worksheetNode);
                }
                else if (worksheetNode.LocalName == GDataSpreadsheetsNameTable.XmlRowCountElement)
                {
                    RowCount = RowCountElement.ParseRowCount(worksheetNode);
                }
            }
        }
    }
}