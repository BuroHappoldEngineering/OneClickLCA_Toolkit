from collections import defaultdict
import json

def tree() -> defaultdict:
    """Return an empty nested defaultdict for building hierarchical key paths."""
    return defaultdict(tree)


def nested_append(node, keys, value):
    """Walk ``keys`` and append ``value`` to the list stored at the final key.

    Intermediate keys are created on demand. If the leaf is not yet a list, it is
    replaced with ``[value]``; otherwise ``value`` is appended.
    """
    if not keys:
        raise ValueError("keys must contain at least one element")

    for key in keys[:-1]:
        node = node[key]

    leaf_key = keys[-1]
    leaf = node[leaf_key]
    if isinstance(leaf, list):
        leaf.append(value)
    else:
        node[leaf_key] = [value]


def to_dict(node):
    """Recursively convert nested ``defaultdict`` trees into plain ``dict`` objects."""
    if isinstance(node, defaultdict):
        return {key: to_dict(child) for key, child in node.items()}
    return node
