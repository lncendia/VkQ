﻿using VkQ.Application.Abstractions.Payments.DTOs;
using VkQ.Application.Abstractions.Payments.Exceptions;
using VkQ.Application.Abstractions.Payments.ServicesInterfaces;
using VkQ.Application.Abstractions.Users.DTOs;
using VkQ.Domain.Abstractions.UnitOfWorks;
using VkQ.Domain.Ordering;
using VkQ.Domain.Transactions.Entities;
using VkQ.Domain.Transactions.Exceptions;
using VkQ.Domain.Transactions.Ordering;
using VkQ.Domain.Transactions.Ordering.Visitor;
using VkQ.Domain.Transactions.Specification;

namespace VkQ.Application.Services.Services.Payments;

public class PaymentService : IPaymentManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentCreatorService _paymentCreatorService;

    public PaymentService(IUnitOfWork unitOfWork, IPaymentCreatorService paymentCreatorService)
    {
        _unitOfWork = unitOfWork;
        _paymentCreatorService = paymentCreatorService;
    }

    /// <exception cref="ErrorCreateBillException"></exception>
    public async Task<CreatePaymentDto> CreateAsync(Guid userId, decimal amount)
    {
        var paymentData = await _paymentCreatorService.CreatePayAsync(userId, amount);
        var transaction = new Transaction(paymentData.BillId, paymentData.PayUrl, amount, userId);
        await _unitOfWork.TransactionRepository.Value.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        return new CreatePaymentDto(transaction.Id, transaction.PaymentSystemId, transaction.Amount,
            transaction.CreationDate, transaction.PaymentSystemUrl);
    }

    /// <exception cref="TransactionNotFoundException"></exception>
    /// <exception cref="BillNotPaidException"></exception>
    /// <exception cref="TransactionAlreadyAcceptedException"></exception>
    /// <exception cref="ErrorCheckBillException"></exception>
    public async Task ConfirmAsync(Guid userId, Guid transactionId)
    {
        var transaction = await _unitOfWork.TransactionRepository.Value.GetAsync(transactionId);
        if (transaction == null) throw new TransactionNotFoundException();
        if (transaction.UserId != userId) throw new TransactionNotFoundException();
        var data = await _paymentCreatorService.CheckPaymentAsync(transaction.PaymentSystemId);
        if (!data) throw new BillNotPaidException(transaction.PaymentSystemId);
        transaction.AcceptPayment();
        await _unitOfWork.TransactionRepository.Value.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }
}