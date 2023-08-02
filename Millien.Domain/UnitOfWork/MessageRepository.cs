﻿using Microsoft.Extensions.Caching.Memory;
using Millien.Domain.DataBase;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;

namespace Millien.Domain.UnitOfWork
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }
}
