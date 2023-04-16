using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.scene
{
    public class MScene
    {
        private readonly List<MNode> nodes = new();
        private readonly Dictionary<string, MNode> namedNodes = new();
        private readonly Dictionary<Type, List<MNode>> typeNodes = new();

        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0f;
        public Vector2 Scale { get; set; } = Vector2.One;
        public MScene Parent { get; private set; }
        public List<MScene> ChildScenes { get; } = new();

        public void AddScene(MScene scene)
        {
            scene.Parent = this;
            ChildScenes.Add(scene);
        }
        
        public void RemoveScene(MScene scene)
        {
            scene.Parent = null;
            ChildScenes.Remove(scene);
        }

        public void AddNode(MNode node)
        {
            nodes.Add(node);

            if (!string.IsNullOrEmpty(node.Name))
            {
                namedNodes[node.Name] = node;
            }

            var nodeType = node.GetType();
            if (!typeNodes.ContainsKey(nodeType))
            {
                typeNodes[nodeType] = new List<MNode>();
            }

            typeNodes[nodeType].Add(node);

            node.Parent = this;
            node.OnAddToScene(this);
        }

        public void RemoveNode(MNode node)
        {
            nodes.Remove(node);

            if (!string.IsNullOrEmpty(node.Name))
            {
                namedNodes.Remove(node.Name);
            }

            var nodeType = node.GetType();
            if (typeNodes.ContainsKey(nodeType))
            {
                typeNodes[nodeType].Remove(node);
            }

            node.OnRemoveFromScene(this);
            node.Parent = null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var node in nodes)
                node.Update(gameTime);
        }

        public void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var node in nodes.OrderBy(n => n.Layer?.Index).ThenBy(n => n.Layer?.Depth))
                node.Render(graphics, spriteBatch, gameTime);
        }

        public List<T> GetNodes<T>(bool recursive = false) where T : MNode
        {
            List<T> result = new List<T>();

            if (typeNodes.TryGetValue(typeof(T), out var nodesOfType))
            {
                result = new List<T>(nodesOfType.Count);
                result.AddRange(nodesOfType.Cast<T>());

                return result;
            }
            
            if (recursive)
                foreach (var scene in ChildScenes)
                    result.AddRange(scene.GetNodes<T>(true));

            return result;
        }

        public T GetNamedNode<T>(string name, bool recursive = false) where T : MNode
        {
            if (namedNodes.TryGetValue(name, out var node) && node is T value)
                return value;
            
            if (recursive)
            {
                foreach (var scene in ChildScenes)
                {
                    var childNode = scene.GetNamedNode<T>(name, true);
                    if (childNode != null)
                        return childNode;
                }
            }

            return null;
        }

    }
}
