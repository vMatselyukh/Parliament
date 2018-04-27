import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { IonicStorageModule } from '@ionic/storage';

import { MyApp } from './app.component';
import {
    HomePage, MainMenuPage, NewItemsListPage, MainListPage,
    RecentlyViewedListPage, MenuPage
} from '../pages/pages';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';

import { DbContext, ParliamentApi, ConfigManager, FileManager, WebServerLinkProvider } from '../providers/providers';
import { HttpModule } from '@angular/http';
import { File } from '@ionic-native/file';
import { FileTransfer } from '@ionic-native/file-transfer';


@NgModule({
  declarations: [
        MyApp,
        HomePage,
        MainMenuPage,
        NewItemsListPage,
        MainListPage,
        RecentlyViewedListPage,
        MenuPage
  ],
  imports: [
    HttpModule,
    BrowserModule,
    IonicModule.forRoot(MyApp, {
        backButtonIcon: "md-arrow-back",
        backButtonText: ''
    }),

    IonicStorageModule.forRoot({
        name: '__parliament',
        driverOrder: ['sqlite', 'indexeddb', 'websql']
    })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
      MyApp,
      HomePage,
      MenuPage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: ErrorHandler, useClass: IonicErrorHandler },
      WebServerLinkProvider,
      DbContext,
      ParliamentApi,
      ConfigManager,
      FileManager,
      File,
      FileTransfer
  ]
})
export class AppModule {}
