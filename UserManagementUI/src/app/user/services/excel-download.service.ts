import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';

@Injectable({
  providedIn: 'root'
})
export class ExcelDownloadService {

  constructor() { }

  downloadExcel(data: any[], fileName: string, columns: { header: string, key: string }[]): void {
    // Create worksheet using only the specified keys
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
      data.map(item => columns.reduce((obj, col) => ({ ...obj, [col.key]: item[col.key] }), {})),
      { header: columns.map(col => col.key) }
    );
    
    // Rename the headers
    const range = XLSX.utils.decode_range(worksheet['!ref']!);
    for (let i = 0; i <= range.e.c; ++i) {
      const address = XLSX.utils.encode_col(i) + '1';
      if (worksheet[address]) {
        worksheet[address].v = columns[i].header;
      }
    }

    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, fileName);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8' });
    const link: HTMLAnchorElement = document.createElement('a');
    link.href = window.URL.createObjectURL(data);
    link.download = `${fileName}_export_${new Date().getTime()}.xlsx`;
    link.click();
  }
}