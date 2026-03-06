# ARCube

HvC Version

## Get Started

- Clone repository
- Download: https://github.com/Iam1337/extOSC
    - Copy Assets/extOSC from extOSC to ARCube's assets


Switch build to Android, ETC2

### Unity VS Linux

The Plastic SCM plugin for Unity is not supported in Linux. Follow these steps to start it in an extra application before loading a Unity project:

- https://www.plasticscm.com/plastic-for-linux

With Ubuntu 24.x there are problems with app permissions - resole like this:

    sudo sysctl -w kernel.apparmor_restrict_unprivileged_userns=0

Permanent:

    kernel.apparmor_restrict_unprivileged_userns=0
