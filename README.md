# Shortcutter 0.1.0 (alpha)
Simple searchbar to quickly find and trigger Unity shortcuts without having to assign key bindings, making the editor more accessible and efficient.

Useful for performing actions without browsing menus or using the cursor, especially shortcuts too obscure and infrequently used to warrant a key binding.

- Install via `Window/Package Manager/Add package from git URL...` and add `https://github.com/TTeig/Shortcutter.git`
- Open the Shortcutter window via `Window/Shortcutter`.
- As you type into the field, the list of suggestions is sorted by closest-ish match.
- Press `up/down` to navigate between suggestions and `enter` to trigger the highlighted shortcut. 
- Recommended: Assign a key binding in `Edit/Shortcuts`. You can also browse the list there for useful shortcuts to use.

It works by creating a temporary shortcut profile, binding `f12` to the selected shortcut, then simulating that key being pressed. Only a tiny fraction of the built in shortcuts have been tested and a lot of them are not expected to work, like 'clutch' shortcuts and context specific ones.

*You can message me on Twitter [@toriously](https://twitter.com/toriously) if you have questions or feedback. -Tori*

![demo gif](https://media.discordapp.net/attachments/512935194093813790/715413414801047612/demo2.gif)