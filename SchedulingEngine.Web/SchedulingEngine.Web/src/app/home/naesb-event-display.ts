import { Component, EventEmitter, Input, Output, OnInit, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ApiService } from '../services/api.service';
import { NaesbEventModel } from '../model/naesb-event.model';
import { Observable } from 'rxjs';

@Component({
    selector: 'naesb-event-display',
    templateUrl: './naesb-event-display.html'
})
export class NaesbEventDisplayComponent implements OnInit, OnChanges {


    //array of months.
    bool = ["true", "false"];


    eventType: string = ""; 
    pipeline: string = "";
    cycle: string = "";
    utility: string = "";

    private _naesbEvents: NaesbEventModel[];
    //private _naesbEventsId: string;


    @Output()
    naesbEventsChange = new EventEmitter();

    @Input()
    set naesbEvents(value: NaesbEventModel[]) {
        this._naesbEvents = value;
        //this.naesbEventsChange.emit(this._naesbEventsId);
    };
    get naesbEvents(): NaesbEventModel[] {
        return this._naesbEvents;
    }

    //@Input()
    //set naesbEventsId(value: string) {
    //    this._naesbEventsId = value;
    //    //this.naesbEventsChange.emit(this._naesbEventsId);
    //};
    //get naesbEventsId(): string {
    //    return this.naesbEventsId;
    //};
    
    ngOnChanges(changes: SimpleChanges) {

    }

    //NaesbEventModels: NaesbEventModel[];

    constructor(private apiService: ApiService) { }

    //ngOnInit() {
    //    this.apiService.getNaesbEvent().subscribe((data) => {
    //        console.log(data);
    //        this.naesbEvents = <any>data;
    //    });
    //}
    ngOnInit() {
        this.getNaesbEventsByPipeline("", "", "", "");
    }

    updateValue(e) {
        //this.naesbEventsId = e.target.value;
        console.log(e);
    }

    //myClickFunction(naesbEvents: any): void {
    //    console.log(naesbEvents);
    //}

    onOff(): void {
        console.log(event);
    }

    getNaesbEventsByPipeline(eventType: string, pipeline: string, cycle: string, utility: string): void {
        this.apiService.getNaesbEventByPipeline(eventType, pipeline, cycle, utility).subscribe((data) => {
            console.log(data);
            this.naesbEvents = <any>data;
        });
    }    

    save(naesbEvent: any): void {

        this.apiService.saveNaesbEvent(naesbEvent).subscribe((reponse) => {
            console.log(reponse);
        }); 

    }
    update(id: number, naesbEvent: any): void {
        
        this.apiService.updateNaesbEvent(id, naesbEvent).subscribe((reponse) => {
            console.log(reponse);
        });

    }

}