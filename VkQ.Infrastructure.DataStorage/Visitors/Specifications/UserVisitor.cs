using System.Linq.Expressions;
using VkQ.Domain.Specifications.Abstractions;
using VkQ.Domain.Users.Entities;
using VkQ.Domain.Users.Specification;
using VkQ.Domain.Users.Specification.Visitor;

namespace VkQ.Infrastructure.DataStorage.Visitors.Specifications;

public class UserVisitor : BaseVisitor<UserModel, IUserSpecificationVisitor, User>, IUserSpecificationVisitor
{
    protected override Expression<Func<UserModel, bool>> ConvertSpecToExpression(
        ISpecification<User, IUserSpecificationVisitor> spec)
    {
        var visitor = new UserVisitor();
        spec.Accept(visitor);
        return visitor.Expr!;
    }

    public void Visit(UserByEmailSpecification specification) => Expr = x => x.Email == specification.Email;
    public void Visit(UserByIdSpecification specification) => Expr = x => x.Id == specification.Id;
}