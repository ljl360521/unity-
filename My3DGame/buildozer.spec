[app]
title = My3DGame
package.name = my3dgame
package.domain = com.mycompany
source.dir = .
source.include_exts = png,jpg,kv,py,atlas,gd,tscn,res,tres,cfg,json
version = 1.0.0
requirements = python3,kivy==2.1.0,pillow,numpy,pyjnius
orientation = landscape
android.api = 33
android.minapi = 24
android.sdk = /root/Android/Sdk
android.ndk = /root/Android/Sdk/ndk/25.2.9519653
android.accept_sdk_license = True
icon.filename = assets/icon.png
permissions = INTERNET,VIBRATE,WAKE_LOCK
android.archs = arm64-v8a
debug_mode = False
