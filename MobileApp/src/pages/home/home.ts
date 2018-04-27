import { Component, ViewChild } from '@angular/core';
import { NavController, SegmentButton, ToastController } from 'ionic-angular';

import { MainListPage, RecentlyViewedListPage, NewItemsListPage, MenuPage } from '../pages';
import { DbContext, ParliamentApi, ConfigManager, FileManager, WebServerLinkProvider } from '../../providers/providers';
import { Config, Person } from '../../models/models';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {

    mainListPage = MainListPage;
    recentlyViewedListPage = RecentlyViewedListPage;
    newItemsListPage = NewItemsListPage;
    config: Config;
    configToDownload: Config;

    @ViewChild('MainListSegmentButton') mainListSegmentButton: SegmentButton;

    constructor(public navCtrl: NavController,
        private dbContext: DbContext,
        private parliamentApi: ParliamentApi,
        public toast: ToastController,
        private configManager: ConfigManager,
        private fileManager: FileManager,
        private linkProvider: WebServerLinkProvider) {
    
    }

    ionViewDidEnter()
    {
        this.mainListSegmentButton.isActive = true;
    }

    ionViewDidLoad()
    {
        this.dbContext.getConfig().then(dbConfig => {
            if (dbConfig == null) {
                this.parliamentApi.getConfig()
                    .then(config => {
                        this.config = config;
                        this.dbContext.saveConfig(config);
                    })
                    .catch(e => this.fileManager.showToast("getConfigError:" + e));
            }
            else {
                console.log("db config isn't null.");
                this.config = dbConfig;

                this.parliamentApi.getConfig()
                    .then(config => {
                        this.configToDownload = this.configManager.getConfigToDownload(this.config, config);
                        this.fileManager.downloadFilesByConfig(this.configToDownload);

                        this.fileManager.showToast("Config to download:" + this.configToDownload);
                    })
                    .catch(e => this.fileManager.showToast("Api get config error:" + e));
            }
        });
    }

    itemClick(person: Person)
    {
        let toast = this.toast.create({
            message: person.Name,
            duration: 8000,
            position: 'bottom'
        });

        toast.present();   
    }

    goToMenu()
    {
        this.navCtrl.push(MenuPage, this.configToDownload);
    }
}
