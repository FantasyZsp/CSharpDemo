using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common;

/// <summary>
/// 表示树形结构的节点接口。
/// </summary>
/// <typeparam name="TLevel">表示层级的类型。</typeparam>
/// <typeparam name="TNode">表示树节点的类型。</typeparam>
public interface ILevelTreeNode<TLevel, TNode> : IComparable<ILevelTreeNode<TLevel, TNode>> where TNode : ILevelTreeNode<TLevel, TNode>
{
    /// <summary>
    /// 获取结点所处的 level。
    /// </summary>
    /// <returns>结点所处的 level。</returns>
    TLevel Level();

    /// <summary>
    /// 推算孩子结点所处的 level。
    /// </summary>
    /// <returns>孩子结点所处的 level。</returns>
    TLevel CalculateChildLevel();

    /// <summary>
    /// 设置孩子节点。
    /// </summary>
    /// <param name="children">属于当前结点的孩子结点。</param>
    void SetChildren(List<TNode> children);

    /// <summary>
    /// 获取孩子节点。
    /// </summary>
    /// <returns>属于当前结点的孩子结点。</returns>
    List<TNode> GetChildren();

    /// <summary>
    /// 获取当前节点。
    /// </summary>
    /// <returns>当前节点。</returns>
    TNode CurrentNode();

    /// <summary>
    /// 将列表转换为树结构。
    /// </summary>
    /// <param name="levelItems">需要转换的集合。</param>
    /// <param name="levelComparison">从 <paramref name="levelItems"/> 找到根节点 level 的比较器。</param>
    /// <returns>已分组结构。</returns>
    public static List<TNode> Transfer2Tree(List<TNode> levelItems, IComparer<TLevel> levelComparison)
    {
        if (typeof(TLevel).IsAssignableFrom(typeof(IComparer<TLevel>)) && levelComparison == null)
        {
            levelComparison = Comparer<TLevel>.Default;
        }

        if (levelItems == null)
            throw new ArgumentNullException(nameof(levelItems));
        if (levelComparison == null)
            throw new ArgumentNullException(nameof(levelComparison));

        if (levelItems.Count == 0)
            return new List<TNode>();

        var rootLevel = levelItems.MinBy(item => item.Level(), levelComparison);


        return Transfer2Tree(rootLevel.Level(), levelItems);
    }

    /// <summary>
    /// 将列表转换为树结构。
    /// </summary>
    /// <param name="rootLevel">根节点 level，或森林顶层相同的 level。</param>
    /// <param name="levelItems">需要转换的集合。</param>
    /// <returns>已分组结构。</returns>
    static List<TNode> Transfer2Tree(TLevel rootLevel, List<TNode> levelItems)
    {
        if (invalidRootLevel(rootLevel))
            throw new ArgumentException("Invalid root level.", nameof(rootLevel));

        if (levelItems == null)
            throw new ArgumentNullException(nameof(levelItems));

        if (levelItems.Count == 0)
            return new List<TNode>();

        var levelGroups = levelItems.GroupBy(item => item.Level()).ToDictionary(g => g.Key, g => g.ToList());

        return Recur2Tree(levelGroups[rootLevel], levelGroups);
    }

    /// <summary>
    /// 递归构造树形结构。
    /// </summary>
    /// <param name="currentLevelGroup">某一层级所有兄弟结点。</param>
    /// <param name="levelGroups">同一森林中从某层级后所有 items。</param>
    /// <returns>构造好的树形结构。</returns>
    static List<TNode> Recur2Tree(List<TNode> currentLevelGroup, Dictionary<TLevel, List<TNode>> levelGroups)
    {
        if (currentLevelGroup == null || currentLevelGroup.Count == 0)
            return new List<TNode>();

        foreach (var item in currentLevelGroup)
        {
            TLevel childLevel = item.CalculateChildLevel();
            var childLevelGroup = levelGroups[childLevel];

            if (childLevelGroup != null && childLevelGroup.Count > 0)
            {
                childLevelGroup.Sort();
                item.SetChildren(childLevelGroup);
                Recur2Tree(childLevelGroup, levelGroups);
            }
        }

        return currentLevelGroup;
    }

    /// <summary>
    /// 将树结构转换为列表。
    /// </summary>
    /// <param name="root">根节点。</param>
    /// <returns>树结构转换后的列表。</returns>
    static List<TNode> Tree2List(TNode root)
    {
        if (root == null)
            throw new ArgumentNullException(nameof(root));

        var resultList = new List<TNode> {root};
        var originalChildren = root.GetChildren();

        if (originalChildren == null)
            return resultList;

        var children = new List<TNode>(originalChildren);

        if (children.Count == 0)
            return resultList;

        Recur2List(resultList, children);

        foreach (var t in resultList)
            t.SetChildren(new List<TNode>());

        return resultList;
    }

    private static void Recur2List(List<TNode> resultList, List<TNode> children)
    {
        if (children != null && children.Count > 0)
        {
            foreach (var child in children)
            {
                if (child != null)
                {
                    resultList.Add(child);
                    Recur2List(resultList, child.GetChildren());
                }
            }
        }
    }

    /// <summary>
    /// 是否是有效结点level。
    /// </summary>
    /// <param name="rootLevel">结点 level。</param>
    /// <returns>无效时返回 true。</returns>
    private static bool invalidRootLevel(TLevel rootLevel)
    {
        return rootLevel == null;
    }
}