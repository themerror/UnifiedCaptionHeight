# UnifiedCaptionHeight

This app makes the default Win32 caption height the same as the UWP, which may make it easier to use touchscreen.

As it is an **experimental** project, you need to use it at your own risk. The author is **NOT** responsible for any damage or data loss.

**⚠⚠You need to sign out to take effect!⚠⚠**

**⚠⚠Do NOT use it if you have screens with different Dpis!⚠⚠**
 
## Design ideas
1. In ```HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics``` there is a value named ```CaptionHeight```, which decides the default catpion height of Win32 apps.
2. The UWP caption height $$Height_{UWP} = 32 * dpi $$
3. The default Win32 height $$Height_{Win32} = \left \lceil 26*dpi+4 \right \rceil $$
4. Therefore, we have $$\frac{-330}{Height_{Win32}} = \frac{result}{Height_{UWP}}$$
5. Hence, $$result = -330 * \frac{Height_{UWP}}{Height_{Win32}} $$

## To-do

- [x] The modification of Package.appxmanifest
- [ ] Use ```ContentDialog``` to promt the user to sign out
- [ ] Use ```ContentDialog``` to display the disclaimer
- [ ] Make everything into a ```Page```(Currently I run into a bug that if I try to use ```rootFrame.Navigate(typeof(MainPage))```, the app will crash)