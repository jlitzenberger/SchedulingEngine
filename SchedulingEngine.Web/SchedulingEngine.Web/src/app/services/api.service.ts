import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { NaesbEventModel } from '../model/naesb-event.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
    
    constructor(private httpClient: HttpClient) { }

    public getNaesbEventByPipeline(eventType: string, pipeline: string, cycle: string, utility: string) {
        //return this.httpClient.get("http://localhost/SchedulingEngine.Web/api/NaesbEvent/GetByPipeline" + eventType + "/" + pipeline + "/" + cycle + "/" + utility);
        return this.httpClient.get("http://localhost/SchedulingEngine.Web/api/NaesbEvent/GetByPipeline?eventType=" + eventType + "&pipeline=" + pipeline + "&cycle=" + cycle + "&utility=" + utility);
    }

    public getNaesbEvent() {
        //return this.httpClient.get("api/NaesbEvent/1");
        return this.httpClient.get("http://localhost/SchedulingEngine.Web/api/NaesbEvent/");
    }

    public saveNaesbEvent(obj: NaesbEventModel) {
        
        //console.log(obj);

        //let value = JSON.stringify(obj);

        //return this.httpClient.post("http://localhost/SchedulingEngine.Web/api/NaesbEvent/", value, {
        //        headers: new HttpHeaders(
        //            { 'Content-Type': 'application/json' }
        //      
        //    }
        //).pipe(
        //    map(data => data)
        //);


        //console.log(obj);
        //let value = JSON.stringify(obj.Cycle);
        //console.log(value);

        //let config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

        return this.httpClient.post("http://localhost/SchedulingEngine.Web/api/NaesbEvent", obj)
         .pipe(
            map(data => data)
         );
    }

    public updateNaesbEvent(id, obj: NaesbEventModel) {

        //console.log(id);
        //console.log(obj);

        //let stuff = JSON.stringify(obj);
        //console.log(stuff);

         //let config = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

        return this.httpClient.put("http://localhost/SchedulingEngine.Web/api/NaesbEvent/" + id, obj)
        .pipe(
            map(data => data)
        );
    }
}
