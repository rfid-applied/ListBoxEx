using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ListBoxExSample.Xaml;

namespace ListBoxExSample
{
    public abstract class Proposal { }
    public class SetFile : Proposal
    {
        public SetFile(string filename, string source)
        {
            _filename = filename;
            _source = source;
        }

        string _filename;
        public string FileName { get { return _filename; } }
        string _source;
        public string Source { get { return _source; } }
    }
    public class RefreshFromFile : Proposal
    {
        public RefreshFromFile(string source) { _source = source; }
        string _source;
        public string Source { get { return _source; } }
    }
    public class SelectElement : Proposal
    {
        public SelectElement(Spencen.Mobile.Markup.XamlElement element) { _element = element; }
        Spencen.Mobile.Markup.XamlElement _element;
        public Spencen.Mobile.Markup.XamlElement Element { get { return _element; } }
    }
    public class ShowError : Proposal
    {
        public ShowError(string context, Exception ex) { _context = context; _ex = ex; }
        string _context;
        Exception _ex;
        public string Context { get { return _context; } }
        public Exception Exception { get { return _ex; } }
    }

    public class Actions
    {
        Model _model;
        Action<Model> _display;

        public Actions(Model model, Action<Model> display)
        {
            _model = model;
            _display = display;
        }

        public void Initialize()
        {
            _display(_model);
        }

        public void SetFile(string filename)
        {
            Proposal action;

            try
            {
                StringBuilder sb = new StringBuilder();
                using (var sr = new System.IO.StreamReader(filename))
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
                string source = sb.ToString();
                action = new SetFile(filename, source);
            }
            catch (Exception e)
            {
                action = new ShowError("Failed to read the file", e);
            }

            _model.Present(action);
            _display(_model);
        }

        public void RefreshFromFile()
        {
            Proposal action;

            if (string.IsNullOrEmpty(_model.FileName))
                action = new ShowError("Please select a file first!", null);
            else
            {
                SetFile(_model.FileName);
                return;
            }

            _model.Present(action);
            _display(_model);
        }

        public void SelectElement(Spencen.Mobile.Markup.XamlElement element)
        {
            Proposal action;

            if (element == null)
                return; // just ignore

            action = new SelectElement(element);
            _model.Present(action);
            _display(_model);
        }

        public void ShowError(string context, Exception e)
        {
            _model.Present(new ShowError(context, e));
            _display(_model);
        }
    }

    public class Model
    {
        string _filename;
        string _source;
        Spencen.Mobile.Markup.XamlElement _tree;
        Spencen.Mobile.Markup.XamlElement _selected;
        Exception _lastError;

        public Model()
        {
        }

        public string FileName { get { return _filename; } }
        public string Source { get { return _source; } }
        public Spencen.Mobile.Markup.XamlElement Tree { get { return _tree; } }
        public Spencen.Mobile.Markup.XamlElement Selected { get { return _selected; } }
        public Exception LastError { get { return _lastError; } }

        public void Present(Proposal action)
        {
            var needsRefresh = false;

            if (action is SetFile)
            {
                _filename = ((SetFile)action).FileName;
                _source = ((SetFile)action).Source;
                needsRefresh = true;
                _lastError = null;
            }
            else if (action is RefreshFromFile)
            {
                _source = ((RefreshFromFile)action).Source;
                needsRefresh = true;
                _lastError = null;
            }
            else if (action is SelectElement)
            {
                var elem = ((SelectElement)action).Element;
                _selected = elem;
                _lastError = null;
            }
            else if (action is ShowError)
            {
                var act = ((ShowError)action);
                _lastError = new Exception(act.Context, act.Exception);
            }
            else
            {
                throw new Exception("Unsupported action type: " + action.GetType().Name);
            }

            if (needsRefresh)
            {
                try
                {
                    var render = new XamlRenderer(null);

                    byte[] byteArray = Encoding.UTF8.GetBytes(_source);
                    using (var stream = new System.IO.MemoryStream(byteArray))
                    {
                        var tree = render.Render(stream);

                        this._selected = tree;
                        this._tree = tree;
                    }
                }
                catch (Exception e)
                {
                    // keep everything else intact
                    this._lastError = new Exception("Failed to refresh from file", e);
                }
            }
        }
    }
}
