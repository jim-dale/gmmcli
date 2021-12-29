using System;

namespace gmmcli.gmdb
{
    /// <summary>
    /// Mapped to the rows in the tblPerson table in the Groove Music database
    /// </summary>
    internal class Person
    {
        public Guid? BingId { get; set; }               // JET_COLUMNID(0x3), "BingId", 16, None
        public string Description { get; set; }         // JET_COLUMNID(0x103), "Description", LongText, Unicode
        public int? Id { get; set; }                    // JET_COLUMNID(0x1), "Id", Long, None
        public uint? MetadataState { get; set; }        // JET_COLUMNID(0x5), "MetadataState", 14, None
        public string Name { get; set; }                // JET_COLUMNID(0x100), "Name", LongText, Unicode
        public int? RowVersion { get; set; }            // JET_COLUMNID(0x6), "RowVersion", Long, None
        public Guid? ServiceMediaId { get; set; }       // JET_COLUMNID(0x2), "ServiceMediaId", 16, None
        public string SortName { get; set; }            // JET_COLUMNID(0x101), "SortName", LongText, Unicode
        public int? SortNameGroupIndex { get; set; }    // JET_COLUMNID(0x4), "SortNameGroupIndex", Long, None
        public string SortNameOverride { get; set; }    // JET_COLUMNID(0x102), "SortNameOverride", LongText, Unicode

        public override string ToString()
        {
            return new { Id, Name, ServiceMediaId }.ToString();
        }
    }
}
