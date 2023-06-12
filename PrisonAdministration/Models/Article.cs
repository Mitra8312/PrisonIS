using System;
using System.Collections.Generic;

public partial class Article
{
    public int IdArticle { get; set; }

    public string TegsArticle { get; set; }

    public string ShortNameArticle { get; set; }

    public string TextArticle { get; set; }

    public DateTime DateOfPublishing { get; set; }

    public TimeSpan TimeOfPublishing { get; set; }

    public int ArticlePublisherId { get; set; }

}
