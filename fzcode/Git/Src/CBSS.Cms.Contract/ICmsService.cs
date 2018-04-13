using System;
using System.Collections.Generic;

namespace CBSS.Tbx.Contract
{
    public interface ITbxService
    {
        Article GetArticle(int id);
        IEnumerable<Article> GetArticleList(ArticleRequest request = null);
        void SaveArticle(Article article);
        void DeleteArticle(List<int> ids);

        Channel GetChannel(int id);
        IEnumerable<Channel> GetChannelList(ChannelRequest request = null);
        void SaveChannel(Channel channel);
        void DeleteChannel(List<int> ids);

        IEnumerable<Tag> GetTagList(TagRequest request = null);
    }
}
