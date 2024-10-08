﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace UtilService.Util;

/// <summary>
/// Collection service
/// </summary>
public static class CollectionService
{
    /// <summary>
    /// Convert IEnumerable to ObservableCollection
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> source) where T : class
    {
        return new ObservableCollection<T>(source);
    }
}

/// <summary>
/// Class to check minimum list itens
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MinimumItemsAttribute : ValidationAttribute
{
    /// <summary>
    /// Minimum item quantity
    /// </summary>
    public int Minimum { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="minimum"></param>
    public MinimumItemsAttribute(int minimum)
    {
        Minimum = minimum;
    }

    /// <summary>
    /// Validate method
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection collection)
        {
            return collection.Count < Minimum ? new ValidationResult(ErrorMessage ?? "It is mandatory to inform items in the collection") : ValidationResult.Success;
        }

        return ValidationResult.Success;
    }

    
}