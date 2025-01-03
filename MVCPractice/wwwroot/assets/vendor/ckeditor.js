import {
    ClassicEditor,
    AccessibilityHelp,
    AutoLink,
    Autosave,
    Bold,
    Code,
    Essentials,
    FontBackgroundColor,
    FontColor,
    FontFamily,
    FontSize,
    Italic,
    Link,
    Paragraph,
    SelectAll,
    SourceEditing,
    SpecialCharacters,
    Strikethrough,
    Underline,
    Undo
} from 'ckeditor5';
import { SlashCommand } from 'ckeditor5-premium-features';

const editorConfig = {
    toolbar: {
        items: [
            'undo',
            'redo',
            '|',
            'sourceEditing',
            'selectAll',
            '|',
            'fontSize',
            'fontFamily',
            'fontColor',
            'fontBackgroundColor',
            '|',
            'bold',
            'italic',
            'underline',
            'strikethrough',
            'code',
            '|',
            'specialCharacters',
            'link',
            '|',
            'accessibilityHelp'
        ],
        shouldNotGroupWhenFull: false
    },
    placeholder: 'Type or paste your content here!',
    plugins: [
        AccessibilityHelp,
        AutoLink,
        Autosave,
        Bold,
        Code,
        Essentials,
        FontBackgroundColor,
        FontColor,
        FontFamily,
        FontSize,
        Italic,
        Link,
        Paragraph,
        SelectAll,
        SourceEditing,
        SpecialCharacters,
        Strikethrough,
        Underline,
        Undo
    ],
    licenseKey: '<YOUR_LICENSE_KEY>',
    mention: {
        feeds: [
            {
                marker: '@',
                feed: [
                    /* See: https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html */
                ]
            }
        ]
    },

};

ClassicEditor
    .create(document.querySelector('#editor'), editorConfig)
    .then(editor => {
        console.log(editor);
    })
    .catch(error => {
        console.error(error);
    });
