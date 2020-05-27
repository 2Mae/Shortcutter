# Shortcutter
Simple searchbar to quickly find and trigger Unity shortcuts without assigning key bindings, including automatically generated shortcuts from assets and user scripts like adding components or accessing menu items.

Install via package manager: *Add package from git URL...* and paste the link to this repository.

Open the Shortcutter window via *Window/Shortcutter* or *Control + T* (can be changed in shortcut manager). 

It works by creating a temporary shortcut profile, binding a key to the selected shortcut, then simulating that key being pressed. Only a tiny fraction of the built in shortcuts have been tested and a lot of them are not expected to work, like 'clutch' shortcuts.