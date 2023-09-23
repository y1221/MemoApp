using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace MemoApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // ********************
        // 保存処理
        // ********************
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileSavePicker();

            // 初期フォルダ
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // 既定の拡張子
            filePicker.DefaultFileExtension = ".txt";

            // サポートするファイルの拡張子
            filePicker.FileTypeChoices.Add("テキスト", new List<string>() { ".txt", ".csv" });
            filePicker.FileTypeChoices.Add("リッチテキスト", new List<string>() { ".rtf" });

            // ファイル名の初期候補
            filePicker.SuggestedFileName = "新規メモ";

            // 主処理
            StorageFile file =  await filePicker.PickSaveFileAsync();

            if (file != null)
            {
                // [保存]ボタンが押された場合（fileがnullでない）
                // txtMemoの内容をファイルに保存する

                await FileIO.WriteTextAsync(file, txtMemo.Text);
            }
        }
    }
}
