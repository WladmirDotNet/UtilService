using System;

namespace UtilService.Util.Model;

/// <summary>
/// Class dedicated to pagination
/// </summary>
public class Pager
{
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalRegistries"></param>
    public Pager(int currentPage, int pageSize, int totalRegistries)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalRegistries = totalRegistries;
        TotalPages = (int)Math.Ceiling((double)TotalRegistries / PageSize);
    }
    /// <summary>
    /// Current page
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// Total pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Total registries in colection
    /// </summary>
    public int TotalRegistries { get; set; }
}