﻿using VkQ.Domain;
using VkQ.Infrastructure.DataStorage.Models;

namespace VkQ.Infrastructure.DataStorage.Factories.Abstractions;

public interface IAggregateFactory<out TAggregate, in TModel> where TAggregate : IAggregateRoot where TModel : IModel
{
    TAggregate Create(TModel model);
}