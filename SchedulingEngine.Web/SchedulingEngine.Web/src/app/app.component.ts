import { Component } from '@angular/core';
import { NaesbEventModel } from './model/naesb-event.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'super fun awesome app';
    
    //public naesbEvents: NaesbEventModel[] = [        
    //    {
    //        Id: "GET",
    //        FileType: "URI_GET",
    //        Cycle: "Cycle",
    //        Pipeline: "Pipeline",
    //        Utility: "Utility",
    //        ProcessedTime: "ProcessedTime",
    //        CycleStart: "CycleStart",
    //        CycleEnd: "CycleEnd",
    //        OffSet: "Offset",
    //        On: "On",
    //        LastUpdateTime: "LastUpdateTime",
    //        LastUpdateUserId: "LastUpdateUserId"
    //    }
    //];

    //public naesbEvents: NaesbEventModel[] = [];

    constructor() {
    }
}
