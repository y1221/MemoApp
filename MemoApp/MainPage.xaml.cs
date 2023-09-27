using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
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

        // ********************
        // 開く処理
        // ********************
        private async void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();

            // 初期フォルダ
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // サポートするファイルの拡張子
            filePicker.FileTypeFilter.Add(".txt");
            filePicker.FileTypeFilter.Add(".csv");

            //主処理
            StorageFile file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                // [開く]ボタンが押された場合（fileがnullでない）
                // ファイルの内容をtxtMemoに書き出す

                try
                {
                    string text = await FileIO.ReadTextAsync(file);
                    txtMemo.Text = text;
                }
                catch (FileNotFoundException ex)
                {
                    await new MessageDialog(ex.Message, "エラー").ShowAsync();
                }

            }
        }

        // ********************
        // コピー処理
        // ********************
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dtPkg = new DataPackage();

            // クリップボード操作
            dtPkg.RequestedOperation = DataPackageOperation.Copy;

            // txtMemoで選択されているテキストをクリップボードにセットする
            dtPkg.SetText(txtMemo.SelectedText);

            // クリップボードにデータをセットする
            Clipboard.SetContent(dtPkg);
        }

        // ********************
        // 切り取り処理
        // ********************
        private void btnCut_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dtPkg = new DataPackage();

            int startPos = txtMemo.SelectionStart;
            int selectedLen = txtMemo.SelectionLength;

            // クリップボード操作
            dtPkg.RequestedOperation = DataPackageOperation.Move;

            // txtMemoで選択されているテキストをクリップボードにセットする
            dtPkg.SetText(txtMemo.SelectedText);

            // クリップボードにデータをセットする
            Clipboard.SetContent(dtPkg);

            // 切り取り後の文字列を作成する
            string strNewMemo = txtMemo.Text.Substring(0, startPos) + txtMemo.Text.Substring(startPos + selectedLen);

            txtMemo.Text = strNewMemo;

            // 切り取り前と同じ位置にカーソルをセットする
            txtMemo.SelectionStart = startPos;
        }

        // ********************
        // 貼り付け処理
        // ********************
        private async void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            // クリップボードから文字列を取得する
            string strMemo = await Clipboard.GetContent().GetTextAsync();

            // txtMemoのカーソルの位置に挿入する
            txtMemo.Text = txtMemo.Text.Insert(txtMemo.SelectionStart, strMemo);
        }
    }
}
