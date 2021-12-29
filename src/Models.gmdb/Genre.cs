
namespace gmmcli.gmdb
{
    /// <summary>
    /// Mapped to the rows in the tblGenre table in the Groove Music database
    /// </summary>
    internal class Genre
    {
        public int? Id { get; set; }                    // JET_COLUMNID(0x1), "Id", Long, None
        public int? RowVersion { get; set; }            // JET_COLUMNID(0x2), "RowVersion", Long, None
        public string Name { get; set; }                // JET_COLUMNID(0x100), "Name", LongText, Unicode
    }
}
