﻿using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace UtilService.Util;

/// <summary>
/// Class dedicated for json operations
/// </summary>
public static class JsonService
{
    /// <summary>
    /// Search node on json nome by name
    /// </summary>
    /// <param name="node"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static JsonNode FindFirstByKeyName(this JsonNode node, string key)
    {
        switch (node)
        {
            case JsonObject jsonObject:
            {
                foreach (var property in jsonObject)
                {
                    if (property.Key == key)
                    {
                        return property.Value;
                    }

                    if (property.Value is not JsonObject && property.Value is not JsonArray)
                        continue;

                    var result = FindFirstByKeyName(property.Value, key);
                    if (result != null)
                    {
                        return result;
                    }
                }

                break;
            }
            case JsonArray jsonArray:
            {
                foreach (var item in jsonArray)
                {
                    if (item == null)
                        continue;

                    var result = FindFirstByKeyName(item, key);
                    if (result != null)
                    {
                        return result;
                    }
                }

                break;
            }
        }

        return null;
    }

    /// <summary>
    /// Search nodes on json nome by name
    /// </summary>
    /// <param name="rootNode"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public static List<JsonNode> FindAllByKeyName(this JsonNode rootNode, string keyName)
    {
        var resultNodes = new List<JsonNode>();
        FindAllByKeyName(rootNode, keyName, resultNodes);
        return resultNodes;
    }

    private static void FindAllByKeyName(JsonNode node, string keyName, List<JsonNode> resultNodes)
    {
        switch (node)
        {
            case JsonObject jsonObject:
            {
                foreach (var property in jsonObject)
                {
                    if (property.Key.Equals(keyName, StringComparison.OrdinalIgnoreCase))
                    {
                        resultNodes.Add(property.Value);
                    }

                    FindAllByKeyName(property.Value, keyName, resultNodes);
                }

                break;
            }
            case JsonArray jsonArray:
            {
                foreach (var item in jsonArray)
                {
                    FindAllByKeyName(item, keyName, resultNodes);
                }

                break;
            }
        }
    }
}