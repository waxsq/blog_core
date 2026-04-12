using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.Records
{
    public record TagCountRecord(List<long> Tags, int Count) : IRequest;
}
