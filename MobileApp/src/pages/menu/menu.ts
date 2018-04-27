import { Component, ViewChild } from '@angular/core';
import { NavController, NavParams, SegmentButton, ToastController, Platform } from 'ionic-angular';
import * as _ from 'lodash';

import { HomePage } from '../pages';
import { ParliamentApi, FileManager } from '../../providers/providers';
import { Config, Person, Track } from '../../models/models';
import { File, DirectoryEntry } from '@ionic-native/file';

const IMAGES_DIR: string = "Politician_Images";
const TRACKS_DIR: string = "Politicians_Tracks";

@Component({
    selector: 'page-menu',
    templateUrl: 'menu.html'
})
export class MenuPage {

    homePage = HomePage;
    configToDownload: Config;

    constructor(public navCtrl: NavController,
        private parliamentApi: ParliamentApi,
        public toast: ToastController,
        private navParams: NavParams,
        private file: File,
        private fileManager: FileManager,
        private platform: Platform) {
        this.configToDownload = this.navParams.data;
    }

    get itemsToDownloadCount(): number {
        let count: number = 0;

        _.forEach(this.configToDownload.Persons, (person: Person) => {
            if (person.ListButtonPicPath.MustBeDownloaded) {
                count = count + 1;
            }

            if (person.MainPicPath.MustBeDownloaded) {
                count = count + 1;
            }

            if (person.SmallButtonPicPath.MustBeDownloaded) {
                count = count + 1;
            }

            _.forEach(person.Tracks, (track: Track) => {
                if (track.MustBeDownloaded) {
                    count = count + 1;
                }
            });
        });

        return count;
    }

    ionViewDidEnter() {

    }

    ionViewDidLoad() {

    }

    createDir(): void {
        this.platform.ready().then(() => {
            this.file.createDir(this.file.dataDirectory, IMAGES_DIR, false).then((directory: DirectoryEntry) => {
                this.fileManager.showToast('ios createDir:' + directory.toURL());
            }).catch((error) => {
                this.fileManager.showToast('ios createDirError:' + error);
            });
        });
    }

    checkDir(): void {
        this.platform.ready().then(() => {

            this.file.checkDir(this.file.dataDirectory, IMAGES_DIR).then(() => {
                this.fileManager.showToast('ios dir already exists:' + this.file.dataDirectory + IMAGES_DIR);
            }).catch((error) => {
                this.fileManager.showToast("ios checkDirError:" + error);
            });
        });
    }
}
