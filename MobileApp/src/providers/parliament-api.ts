import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Config } from '../models/models';
import { WebServerLinkProvider } from '../providers/providers';

@Injectable()
export class ParliamentApi {
    //private baseUrl = "http://localhost:2101/Api/Parliament/";
    private baseUrl = this.linkProvider.webServerBaseUrl; //"http://11fa7543-0ee0-4-231-b9ee.azurewebsites.net/Api/Parliament/";
    

    constructor(private http: Http,
                private linkProvider: WebServerLinkProvider) {
    }

    getConfig() : Promise<Config> {
        return new Promise((resolve, reject) => {
            this.http.get(this.baseUrl + "getConfig").subscribe(
                (data) => {
                    var obj = JSON.parse(data.text()) as Config;
                    resolve(obj);
                },
                (error) => {
                    reject(error);
                });
        });
    }

    getFileByUrl(url: string): Promise<string> {
        return new Promise((resolve, reject) => {
            this.http.get(this.baseUrl + "getFile?url="+ url).subscribe(
                (data) => {
                    //var obj = JSON.parse(data.text()) as Config;
                    resolve(data);
                },
                (error) => {
                    reject(error);
                });
        });
    }
}